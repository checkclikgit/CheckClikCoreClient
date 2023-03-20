using CheckClikClient;
using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web; 

namespace Customer.Utils
{
    public static class UploadLocations
    {
        public const string
            ProfilePictures = "/ProfilePictures/",
            Stores = "/Stores/",
            Products = "/Products/",
            HomePage = "/HomePage/",
            Advertisements = "/Advertisements/";
    }
    public static class PaymentFor
    {
        public const string
            Advertisements = "A",
            Vouchers = "C",
            Subscriptions = "S",
            Renewals = "R",
            Access = "E",
            AccessRenewal = "F",
            Orders = "O";
    }
    public class Common
    {
        private static string paymentURL = "";
        private static string AuthenticationUserId = "";
        private static string AuthenticationPwd = "";
        private static string AuthenticationEntityId = "";
        private static string PAEntityId = "";
        private readonly AppSettingsKeysInfo _options;
        public Common(IOptions<AppSettingsKeysInfo> options)
        {
            _options = options.Value;
             paymentURL = _options.paymentURL;
            AuthenticationUserId = _options.AuthenticationUserId;
            AuthenticationPwd = _options.AuthenticationPwd;
            AuthenticationEntityId = _options.AuthenticationEntityId;
            PAEntityId = _options.PAEntityId; 

        }
        //static string paymentURL = ConfigurationManager.AppSettings["paymentURL"];
        //static string AuthenticationUserId = ConfigurationManager.AppSettings["AuthenticationUserId"];
        //static string AuthenticationPwd = ConfigurationManager.AppSettings["AuthenticationPwd"];
        //static string AuthenticationEntityId = ConfigurationManager.AppSettings["AuthenticationEntityId"];
        //static string PAEntityId = ConfigurationManager.AppSettings["PAEntityId"];
         

        //public static FileAttribs PrepareFileAttributes(HttpPostedFileBase objFile)
        //{ 
        //    Stream strm = objFile.InputStream;
        //    byte[] bytes = StreamToBytes(strm);
        //    FileAttribs _fileAttr = new FileAttribs();
        //    _fileAttr.Base64FileData = Convert.ToBase64String(bytes);
        //    _fileAttr.FileName = objFile.FileName;
        //    return _fileAttr;
        //}

        public static byte[] StreamToBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        /*
        public static Dictionary<string, dynamic> RequestPayment(decimal amount, string InvoiceNo, string Email, string recurringType)
        {
            Dictionary<string, dynamic> responseData;
            string data = "authentication.userId=" + AuthenticationUserId +
                "&authentication.password=" + AuthenticationPwd +
                "&authentication.entityId=" + AuthenticationEntityId +
                "&amount=" + amount +
                "&currency=SAR" +
                "&paymentType=DB" +
                "&testMode=EXTERNAL" +
                "&merchantTransactionId=" + InvoiceNo +
                "&customer.email=" + Email;
            if(recurringType== "INITIAL")
            {
                data += "&recurringType=INITIAL" + "&createRegistration=true";
            }
            //else
            //{
            //    data += "&recurringType=REPEATED";
            //}

            string url = paymentURL;
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                var s = new JavaScriptSerializer();
                responseData = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());
                
                reader.Close();
                dataStream.Close();
            }
            return responseData;
        }
        */

        /*
        public static Dictionary<string, dynamic> RequestPaymentStatus(object id)
        {
            Dictionary<string, dynamic> responseData;
            string data = "authentication.userId=" + AuthenticationUserId +
                "&authentication.password=" + AuthenticationPwd +
                "&authentication.entityId=" + AuthenticationEntityId;
            string url = paymentURL + "/" + id + "/payment?" + data;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                var s = new JavaScriptSerializer();
                responseData = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());
                reader.Close();
                dataStream.Close();
            }
            return responseData;
        }
        */
        public static string generateInvoiceNo(int storeId, Int64 userId, String PaymentReg)
        {
            int rno = getRandomNumber();
            TimeZoneInfo AST = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time");
            DateTime astTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, AST);
            string dt = astTime.ToString("ddMMyyhms");
            string invoiceNo = PaymentReg.ToUpper() + "-" + storeId + "-" + userId + "-" + dt + "-" + rno;
            return invoiceNo;
        }
        public static int getRandomNumber()
        {
            Random r = new Random();
            return r.Next(1000, 9999);
        }
        /*
        public static string proceedPayment(string InvoiceNo, Decimal Price, string UserEmail, long UserId, String TransactionType, string recurringType)
        {
            Dictionary<string, dynamic> responseData;

            responseData = Utils.Common.RequestPayment(Price, InvoiceNo, UserEmail, recurringType);
            string checkoutID = responseData["id"];
            //TempData["tempCheckOutId"] = checkoutID;
            //Save Logs in HyperPay
            HyperPayLogsDTO objH = new HyperPayLogsDTO();
            objH.CheckOutId = responseData["id"];
            objH.FlagId = 1;
            objH.RequestData = "";
            objH.RequestType = 1; //1 Request 2 Response
            objH.TransactionDescription = responseData["result"]["description"];
            objH.TransactionId = "";
            objH.TransactionType = TransactionType;
            objH.UserId = UserId;
            string strRespData = JsonConvert.SerializeObject(responseData);
            objH.JsonLog = strRespData;
            Task ts = RecordHyperPayLog(objH);
            HttpContext.Current.Session["LogObj"] = new TransSummaryDTO() { Price = Price.ToString(), InvoiceNo = InvoiceNo, PayFor = TransactionType, Msg = "Continue with payment of " + Price.ToString() + " SAR against " + TransactionType + " with Invoice No: " + InvoiceNo };
            //End Hyperpay Logs

            return checkoutID;
        }
        */
        //public static async Task RecordHyperPayLog(HyperPayLogsDTO objH)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        CommonHeader.setHeaders(client);
        //        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/HyperPayLogsAPI/SaveHyperPayLogs", objH);
        //    }
        //}
        /*
        public static Dictionary<string, dynamic> MakeRecurringPayment(string amount, string transactionID, string InvoiceNo)
        {
            Dictionary<string, dynamic> responseData;
            
            string data = "authentication.userId=" + AuthenticationUserId +
                "&authentication.password=" + AuthenticationPwd +
                "&authentication.entityId=" + AuthenticationEntityId +
                "&amount=" + amount +
                "&currency=SAR" +
                "&paymentType=DB" +
                "&testMode=EXTERNAL" +
                "&recurringType=REPEATED" +
                "&merchantTransactionId=" + InvoiceNo +
                "&customer.email=test@email.com";

            string url = "https://test.oppwa.com/v1/registrations/"+transactionID+"/payments";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    var s = new JavaScriptSerializer();
                    responseData = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());
                    reader.Close();
                    dataStream.Close();
                }
            }
            catch(Exception Ex)
            {
                string msg = Ex.Message;
                responseData = null;
            }
            return responseData;
        }
        */
        //public  string proceedPaymentPA(string InvoiceNo, Decimal Price, string UserEmail, long UserId, String TransactionType, string recurringType)
        //{
        //    Dictionary<string, dynamic> responseData;

        //    responseData = Utils.Common.RequestPaymentPA(Price, InvoiceNo, UserEmail, recurringType);
        //    string checkoutID = responseData["id"];
        //    //TempData["tempCheckOutId"] = checkoutID;
        //    //Save Logs in HyperPay
        //    HyperPayLogsDTO objH = new HyperPayLogsDTO();
        //    objH.CheckOutId = responseData["id"];
        //    objH.FlagId = 1;
        //    objH.RequestData = "";
        //    objH.RequestType = 1; //1 Request 2 Response
        //    objH.TransactionDescription = responseData["result"]["description"];
        //    objH.TransactionId = "";
        //    objH.TransactionType = TransactionType;
        //    objH.UserId = UserId;
        //    string strRespData = JsonConvert.SerializeObject(responseData);
        //    objH.JsonLog = strRespData;
        //    Task ts = RecordHyperPayLog(objH);
        //    ////HttpContext.Current.Session["LogObj"] = new TransSummaryDTO() { Price = Price.ToString(), InvoiceNo = InvoiceNo, PayFor = TransactionType, Msg = "Continue with payment of " + Price.ToString() + " SAR against " + TransactionType + " with Invoice No: " + InvoiceNo };
        //    //HttpContext.Current.Session["LogObj"] = new TransSummaryDTO() { Price = Price.ToString(), InvoiceNo = InvoiceNo, PayFor = TransactionType, Msg = "Continue with payment of " + Price.ToString() + " SAR against " + TransactionType };// + " with Invoice No: " + InvoiceNo };
        //    HttpContext.Session.SetString("LogObj", new TransSummaryDTO() { Price = Price.ToString(), InvoiceNo = InvoiceNo, PayFor = TransactionType, Msg = "Continue with payment of " + Price.ToString() + " SAR against " + TransactionType };);
        //    //HttpContext.Current.Session["LogObj"] = new TransSummaryDTO() { Price = Price.ToString(), InvoiceNo = InvoiceNo, PayFor = TransactionType, Msg = "Continue with payment of " + Price.ToString() + " SAR against " + TransactionType };// + " with Invoice No: " + InvoiceNo };

        //    //End Hyperpay Logs

        //    return checkoutID;
        //}

        public static Dictionary<string, dynamic> RequestPaymentPA(decimal amount, string InvoiceNo, string Email, string recurringType)
        {
            Dictionary<string, dynamic> responseData;
            string data = "authentication.userId=" + AuthenticationUserId +
                "&authentication.password=" + AuthenticationPwd +
                "&authentication.entityId="+ PAEntityId +
                "&amount=" + amount +
                "&currency=SAR" +
                "&paymentType=PA" +
                "&testMode=EXTERNAL" +
                "&merchantTransactionId=" + InvoiceNo +
                "&customer.email=" + Email;            

            string url = paymentURL;
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                //var s = new JavaScriptSerializer();
                //responseData = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());

                responseData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(reader.ReadToEnd());
                reader.Close();
                dataStream.Close();
            }
            return responseData;
        }

        public static Dictionary<string, dynamic> RequestPaymentCapture(decimal amount, string InvoiceNo, string Email, string recurringType, string Id)
        {
            string hello = "Hello";
            Dictionary<string, dynamic> responseData;
            string data = "authentication.userId=" + AuthenticationUserId +
                "&authentication.password=" + AuthenticationPwd +
                "&authentication.entityId="+ PAEntityId + // AuthenticationEntityId +
                "&amount=" + amount +
                "&currency=SAR" +
                "&paymentType=CP" +
                "&testMode=EXTERNAL";
                //    +
                //"&merchantTransactionId=" + InvoiceNo +
                //"&customer.email=" + Email;

            string url = "https://test.oppwa.com/v1/payments/"+Id;
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                //var s = new JavaScriptSerializer();
                //responseData = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());

                responseData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(reader.ReadToEnd());

                reader.Close();
                dataStream.Close();
            }
            return responseData;
        }

        public static Dictionary<string, dynamic> RequestPaymentStatusPA(object id)
        {
            Dictionary<string, dynamic> responseData;
            string data = "authentication.userId=" + AuthenticationUserId +
                "&authentication.password=" + AuthenticationPwd +
                "&authentication.entityId=" + PAEntityId;  // AuthenticationEntityId;
            string url = paymentURL + "/" + id + "/payment?" + data;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                //var s = new JavaScriptSerializer();
                //responseData = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());
                responseData = JsonConvert.DeserializeObject< Dictionary<string, dynamic>>(reader.ReadToEnd());
                reader.Close();
                dataStream.Close();
            }
            return responseData;
        }
		#region Initial & Recurring Payment
		//public static string proceedPayment(string InvoiceNo, Decimal Price, string UserEmail, long UserId, String TransactionType, string recurringType)
		//{
		//	Dictionary<string, dynamic> responseData;

		//	responseData = Utils.Common.RequestPayment(Price, InvoiceNo, UserEmail, recurringType);
		//	string checkoutID = responseData["id"];
		//	//TempData["tempCheckOutId"] = checkoutID;
		//	//Save Logs in HyperPay
		//	HyperPayLogsDTO objH = new HyperPayLogsDTO();
		//	objH.CheckOutId = responseData["id"];
		//	objH.FlagId = 1;
		//	objH.RequestData = "";
		//	objH.RequestType = 1; //1 Request 2 Response
		//	objH.TransactionDescription = responseData["result"]["description"];
		//	objH.TransactionId = "";
		//	objH.TransactionType = TransactionType;
		//	objH.UserId = UserId;
		//	string strRespData = JsonConvert.SerializeObject(responseData);
		//	objH.JsonLog = strRespData;
		//	Task ts = RecordHyperPayLog(objH);
		//	HttpContext.Current.Session["LogObj"] = new TransSummaryDTO() { Price = Price.ToString(), InvoiceNo = InvoiceNo, PayFor = TransactionType, Msg = "Continue with payment of " + Price.ToString() + " SAR against " + TransactionType + " with Invoice No: " + InvoiceNo };
		//	//End Hyperpay Logs

		//	return checkoutID;
		//}
		public static Dictionary<string, dynamic> RequestPayment(decimal amount, string InvoiceNo, string Email, string recurringType)
		{
			Dictionary<string, dynamic> responseData;
			string data = "authentication.userId=" + AuthenticationUserId +
				"&authentication.password=" + AuthenticationPwd +
				"&authentication.entityId=" + AuthenticationEntityId +
				"&amount=" + amount +
				"&currency=SAR" +
				"&paymentType=DB" +
				"&testMode=EXTERNAL" +
				"&merchantTransactionId=" + InvoiceNo +
				"&customer.email=" + Email;
			if (recurringType == "INITIAL")
			{
				data += "&recurringType=INITIAL" + "&createRegistration=true";
			}
			
			string url = paymentURL+ "/v1/checkouts";
			byte[] buffer = Encoding.ASCII.GetBytes(data);
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			Stream PostData = request.GetRequestStream();
			PostData.Write(buffer, 0, buffer.Length);
			PostData.Close();

			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				Stream dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);
                //var s = new JavaScriptSerializer();
                //responseData = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());

                responseData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(reader.ReadToEnd());
                reader.Close();
				dataStream.Close();
			}
			return responseData;
		}

		public static Dictionary<string, dynamic> RequestPaymentStatus(object id)
		{
			Dictionary<string, dynamic> responseData;
			string data = "authentication.userId=" + AuthenticationUserId +
				"&authentication.password=" + AuthenticationPwd +
				"&authentication.entityId=" + AuthenticationEntityId;
			string url = paymentURL + "/v1/checkouts/" + id + "/payment?" + data; ///{checkoutId}/payment
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.Method = "GET";
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				Stream dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);
                //var s = new JavaScriptSerializer();
                //responseData = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());

                responseData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(reader.ReadToEnd());
                reader.Close();
				dataStream.Close();
			}
			return responseData;
		}

		public static Dictionary<string, dynamic> MakeRecurringPayment(string amount, string transactionID, string InvoiceNo)
		{
			Dictionary<string, dynamic> responseData;

			string data = "authentication.userId=" + AuthenticationUserId +
				"&authentication.password=" + AuthenticationPwd +
				"&authentication.entityId=" + AuthenticationEntityId +
				"&amount=" + amount +
				"&currency=SAR" +
				"&paymentType=DB" +
				"&testMode=EXTERNAL" +
				"&recurringType=REPEATED" +
				"&merchantTransactionId=" + InvoiceNo +
				"&customer.email=test@email.com";

			string url = "https://test.oppwa.com/v1/registrations/" + transactionID + "/payments";
			byte[] buffer = Encoding.ASCII.GetBytes(data);
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			Stream PostData = request.GetRequestStream();
			PostData.Write(buffer, 0, buffer.Length);
			PostData.Close();
			try
			{
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					Stream dataStream = response.GetResponseStream();
					StreamReader reader = new StreamReader(dataStream);
                    //var s = new JavaScriptSerializer();
                    //responseData = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());

                    responseData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(reader.ReadToEnd());

                    reader.Close();
					dataStream.Close();
				}
			}
			catch (Exception Ex)
			{
				string msg = Ex.Message;
				responseData = null;
			}
			return responseData;
		}
        public static DateTime KSA_DateTime()
        {
            TimeZoneInfo KSATimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            DateTime utc = DateTime.UtcNow;
            DateTime KSA = TimeZoneInfo.ConvertTimeFromUtc(utc, KSATimeZone);
            return KSA;
        }

    }
}
#endregion