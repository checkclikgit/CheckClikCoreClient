using CheckClickClient;
using CheckClikClient;
using CheckClikClient.Controllers;
using CheckClikClient.Handlers;
using CheckClikClient.Models;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolrNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Controllers
{
    public class MyOrdersController : BaseController
    {
        //string apiurl = ConfigurationManager.AppSettings["apiurl"];
        //string apiImages = ConfigurationManager.AppSettings["UploadLocation"];
        //string baseUrl = ConfigurationManager.AppSettings["baseUrl"];
        //string currentUrl = "";

        //public MyOrdersController()
        //{
        //    ViewBag.ImagePath = apiurl + apiImages + UploadLocations.Stores;
        //    ViewBag.ProductImages = apiurl + apiImages + UploadLocations.Products;

        //    var baseURL = baseUrl.ToLower();
        //    currentUrl = System.Web.HttpContext.Current.Request.Url.ToString().ToLower();// Current.Request.Url.AbsolutePath;
        //    currentUrl = currentUrl.Replace(baseURL, baseURL + "ar/");
        //    ViewBag.CurrentURL = currentUrl;
        //}

        string apiurl = "";
        string apiImages = "";
        string baseUrl = "";
        string currentUrl = "";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;
        private readonly IViewRenderService _viewRenderService;

        public MyOrdersController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService) : base(httpContextAccessor, errorHandler, options, commonHeader)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options.Value;

            var baseURL = _options.baseUrl;
            currentUrl = _httpContextAccessor.HttpContext.Request.PathBase.ToString();
            currentUrl = currentUrl.Replace(baseURL, baseURL + "ar/");
            apiurl = _options.apiurl;
            apiImages = _options.UploadLocation;

            ViewBag.CurrentURL = currentUrl;
            ViewBag.ImagePath = apiurl + apiImages + UploadLocations.Stores;
            ViewBag.ProductImages = apiurl + apiImages + UploadLocations.Products;
            //TempData["ProductImages"] = apiurl + apiImages + UploadLocations.Products;

            _errorHandler = errorHandler;
            _commonHeader = commonHeader;
            _viewRenderService = viewRenderService;
        }

        // GET: Pending MyOrders      
        //[Route("myorders/index")]
        public async Task<ActionResult> Index(Int64 Id = 0)
        {
            using (HttpClient client = new HttpClient())
            {
                bool status = false;
                _commonHeader.setHeaders(client);
                try
                {
                    UserHistoryByBranchTypeUserOrderIdDTO objNew = new UserHistoryByBranchTypeUserOrderIdDTO();
                    MyOrdersDTO obj = new MyOrdersDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    if (login == null)
                    {
                        return RedirectToAction("Index", "User"); // later call login form
                    }
                    else
                    {
                        obj.Id = login.UserId;
                        objNew.Id = login.UserId;
                        TempData.Remove("ReturnItems_DataTable_" + login.UserId);
                    }
                    //obj.Id = 13;
                    obj.Type = 1;
                    objNew.Type = 1;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/UserHistoryOrdersByBranchTypeUserOrderId", objNew);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        var a = apiRes.SelectToken("Data").ToString();
                        //var b = JArray.Parse(a);
                        //var c = b.SelectToken("Result").ToString();


                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                        var lstUserPendingOrders = JsonConvert.DeserializeObject<List<MyOrdersDTO>>(a);
                        if (lstUserPendingOrders != null)
                        {
                            //if (lstUserPendingOrders.Count != 0)
                            //{
                            //    obj.UsersPendingOrdersList = lstUserPendingOrders;                               
                            //}
                            //else
                            //    obj.UsersPendingOrdersList = null;

                            //TempData["TempPendingOrderList"] = lstUserPendingOrders;
                            //TempData["TempProcessingOrderList"] = null;
                            //TempData["TempHistoryOrderList"] = null;
                        }
                    }
                    obj.OrderId = Id;
                    return View(obj);
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    //AssignValuesToDTO.AssingDToValues(err, ex, "UserList/IndexGet", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    return RedirectToAction("Error");
                }
            }
        }

        public async Task<List<MyOrdersDTO>> GetOrderDetailsAsync(long orderid, int type)
        {
            List<MyOrdersDTO> lstMyOrdersDTO = new List<MyOrdersDTO>();

            bool status = false;
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);

                UserHistoryByBranchTypeUserOrderIdDTO objNew = new UserHistoryByBranchTypeUserOrderIdDTO();
                MyOrdersDTO obj = new MyOrdersDTO();
                SessionDTO login = AppUtils.UserLogin;

                obj.Id = login.UserId;
                objNew.Id = login.UserId;

                obj.OrderId = orderid;
                obj.Type = type;
                objNew.OrderId = orderid;
                objNew.Type = type;
                try
                {
                    //   HttpResponseMessage responseMessage1 = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/UserHistoryOrdersByBranchTypeUserOrderId", objNew);

                    //HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/UserHistoryOrders", obj);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/UserHistoryOrdersByBranchTypeUserOrderId", objNew);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        var a = apiRes.SelectToken("Data").ToString();

                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                        var lstUserPendingOrders = JsonConvert.DeserializeObject<List<MyOrdersDTO>>(a);
                        if (lstUserPendingOrders != null)
                        {
                            if (lstUserPendingOrders.Count != 0)
                                obj.UsersPendingOrdersList = lstUserPendingOrders;
                            //else
                            //    obj.UsersPendingOrdersList = null;

                            if (type == 1 && orderid == 0)
                            {
                                TempData["TempPendingOrderList"] = lstUserPendingOrders;
                            }
                            if (type == 2 && orderid == 0)
                            {
                                TempData["TempProcessingOrderList"] = lstUserPendingOrders;
                            }
                            else if (type == 3 && orderid == 0)
                            {
                                TempData["TempHistoryOrderList"] = lstUserPendingOrders;
                            }
                            lstMyOrdersDTO = lstUserPendingOrders;

                        }
                    }
                    obj.Type = type;
                    TempData.Remove("ReturnItems_DataTable_" + login.UserId);
                }
                catch (Exception ex)
                {

                }
            }
            return lstMyOrdersDTO;
        }

        public async Task<ActionResult> OrderDetails(long orderid, int type)
        {
            using (HttpClient client = new HttpClient())
            {
                bool status = false;
                _commonHeader.setHeaders(client);
                try
                {
                    UserHistoryByBranchTypeUserOrderIdDTO objNew = new UserHistoryByBranchTypeUserOrderIdDTO();
                    MyOrdersDTO obj = new MyOrdersDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    if (login == null)
                        return RedirectToAction("Index", "User"); // later call login form
                    else
                        obj.Id = login.UserId;
                    objNew.Id = login.UserId;

                    obj.OrderId = orderid;
                    obj.Type = type;
                    objNew.OrderId = orderid;
                    objNew.Type = type;

                    //HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/UserHistoryOrders", obj);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/UserHistoryOrdersByBranchTypeUserOrderId", objNew);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        var a = apiRes.SelectToken("Data").ToString();

                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                        var lstUserPendingOrders = JsonConvert.DeserializeObject<List<MyOrdersDTO>>(a);
                        if (lstUserPendingOrders != null)
                        {
                            if (lstUserPendingOrders.Count != 0)
                                obj.UsersPendingOrdersList = lstUserPendingOrders;
                            ////else
                            ////    obj.UsersPendingOrdersList = null;

                            //if (type == 1 && orderid == 0)
                            //{
                            //    TempData["TempPendingOrderList"] = lstUserPendingOrders;
                            //}
                            //if (type == 2 && orderid == 0)
                            //{
                            //    TempData["TempProcessingOrderList"] = lstUserPendingOrders;
                            //}
                            //else if (type == 3 && orderid == 0)
                            //{
                            //    TempData["TempHistoryOrderList"] = lstUserPendingOrders;
                            //}

                        }
                    }
                    obj.Type = type;
                    TempData.Remove("ReturnItems_DataTable_" + login.UserId);
                     
                    obj.UrlPath = apiurl + apiImages + UploadLocations.Products;
                    //return PartialView("OrderDetailsPV", obj);

                    var result = await _viewRenderService.RenderToStringAsync("MyOrders/OrderDetailsPV", obj);
                    return Json(new { result = result });
                    //return Json(true);
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    //AssignValuesToDTO.AssingDToValues(err, ex, "UserList/IndexGet", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    //return RedirectToAction("Error");
                    return Json(false);
                }
            }
        }

        public ActionResult OrderDetailsID(long orderid, int type)
        {
            TempData["Orderid"] = orderid;
            TempData["Type"] = type;
            return RedirectToAction("OrderDetails");
        }

        [HttpPost]
        public async Task<JsonResult> PVPendingOrderlist(int pageNumber, int SortId/*string model*/)
        {
            //int pageNumber = 1, SortId = 1;
            //var res = await OrderDetails(0, 1);
            //List<MyOrdersDTO>  lstMyOrdersDTO = await GetOrderDetailsAsync(0, 1);
            List<MyOrdersDTO> lstMyOrdersDTO = new List<MyOrdersDTO>();
            int orderid = 0;
            int type = 1;
            bool status = false;
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);

                UserHistoryByBranchTypeUserOrderIdDTO objNew = new UserHistoryByBranchTypeUserOrderIdDTO();
                SessionDTO login = AppUtils.UserLogin;

                objNew.Id = login.UserId;
                objNew.OrderId = orderid;
                objNew.Type = type;

                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/UserHistoryOrdersByBranchTypeUserOrderId", objNew);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        var a = apiRes.SelectToken("Data").ToString();

                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                        var lstUserPendingOrders = JsonConvert.DeserializeObject<List<MyOrdersDTO>>(a);
                        if (lstUserPendingOrders != null)
                        {
                            lstMyOrdersDTO = lstUserPendingOrders; 
                        }
                    }
                    TempData.Remove("ReturnItems_DataTable_" + login.UserId);
                }
                catch (Exception ex)
                {

                }
            }








            MyOrdersDTO obj1 = new MyOrdersDTO();
            var PageSize = decimal.Parse(_options.PageSize);
            List<MyOrdersDTO> list = new List<MyOrdersDTO>();
            list = lstMyOrdersDTO;//(List<MyOrdersDTO>)TempData.Peek("TempPendingOrderList");
            int toc = list != null && list.Count() > 0 ? list.Count : 0;
            int count = toc / Convert.ToInt32(PageSize);
            if (list != null && list.Count() > 0)
                list = pageNumber == 1 ? list.Take(Convert.ToInt32(PageSize)).ToList() : list.Skip(((pageNumber + Convert.ToInt32(PageSize)) - 2)).Take(Convert.ToInt32(PageSize)).ToList();
            obj1.UsersPendingOrdersList = list;
            obj1.PageNumber = pageNumber;
            obj1.pagingNumber = Convert.ToDecimal(Convert.ToDecimal(toc) / PageSize) > Convert.ToDecimal(Convert.ToInt32(Convert.ToDecimal(toc) / PageSize)) ? Convert.ToInt32((toc / PageSize)) + 1 : Convert.ToInt32((toc / PageSize));
            //return PartialView("PVPendingOrders", obj);

            obj1.UrlPath = apiurl + apiImages + UploadLocations.Products;

            var result = await _viewRenderService.RenderToStringAsync("MyOrders/PVPendingOrders", obj1);
            return Json(new { result = result });

        }

        [HttpPost]
        public async Task<ActionResult> PVProcessingOrderlist(int pageNumber, int SortId)
        {
            ////if(TempData["TempProcessingOrderList"] == null)
            //await OrderDetails(0, 2);


            List<MyOrdersDTO> lstMyOrdersDTO = new List<MyOrdersDTO>();
            int orderid = 0;
            int type = 2;
            bool status = false;
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);

                UserHistoryByBranchTypeUserOrderIdDTO objNew = new UserHistoryByBranchTypeUserOrderIdDTO();
                SessionDTO login = AppUtils.UserLogin;

                objNew.Id = login.UserId;
                objNew.OrderId = orderid;
                objNew.Type = type;

                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/UserHistoryOrdersByBranchTypeUserOrderId", objNew);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        var a = apiRes.SelectToken("Data").ToString();

                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                        var lstUserPendingOrders = JsonConvert.DeserializeObject<List<MyOrdersDTO>>(a);
                        if (lstUserPendingOrders != null)
                        {
                            lstMyOrdersDTO = lstUserPendingOrders;
                        }
                    }
                    TempData.Remove("ReturnItems_DataTable_" + login.UserId);
                }
                catch (Exception ex)
                {

                }
            }






            MyOrdersDTO obj = new MyOrdersDTO();
            var PageSize = decimal.Parse(_options.PageSize);
            List<MyOrdersDTO> list = new List<MyOrdersDTO>();
            list = lstMyOrdersDTO;//(List<MyOrdersDTO>)TempData.Peek("TempProcessingOrderList");
            int toc = list != null && list.Count() > 0 ? list.Count : 0;
            int count = toc / Convert.ToInt32(PageSize);
            if (list != null && list.Count() > 0)
                list = pageNumber == 1 ? list.Take(Convert.ToInt32(PageSize)).ToList() : list.Skip(((pageNumber + Convert.ToInt32(PageSize)) - 2)).Take(Convert.ToInt32(PageSize)).ToList();
            obj.UsersProcessingOrdersList = list;
            obj.PageNumber = pageNumber;
            obj.pagingNumber = Convert.ToDecimal(Convert.ToDecimal(toc) / PageSize) > Convert.ToDecimal(Convert.ToInt32(Convert.ToDecimal(toc) / PageSize)) ? Convert.ToInt32((toc / PageSize)) + 1 : Convert.ToInt32((toc / PageSize));
            //return PartialView("PVProcessingOrders", obj);
            obj.UrlPath = apiurl + apiImages + UploadLocations.Products;
            var result = await _viewRenderService.RenderToStringAsync("MyOrders/PVProcessingOrders", obj);
            return Json(new { result = result });

        }
        [HttpPost]
        public async Task<ActionResult> PVHistoryOrderlist(int pageNumber, int SortId)
        {
            //// if (TempData["TempHistoryOrderList"] == null)
            //await OrderDetails(0, 3);

            List<MyOrdersDTO> lstMyOrdersDTO = new List<MyOrdersDTO>();
            int orderid = 0;
            int type = 3;
            bool status = false;
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);

                UserHistoryByBranchTypeUserOrderIdDTO objNew = new UserHistoryByBranchTypeUserOrderIdDTO();
                SessionDTO login = AppUtils.UserLogin;

                objNew.Id = login.UserId;
                objNew.OrderId = orderid;
                objNew.Type = type;

                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/UserHistoryOrdersByBranchTypeUserOrderId", objNew);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        var a = apiRes.SelectToken("Data").ToString();

                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                        var lstUserPendingOrders = JsonConvert.DeserializeObject<List<MyOrdersDTO>>(a);
                        if (lstUserPendingOrders != null)
                        {
                            lstMyOrdersDTO = lstUserPendingOrders;
                        }
                    }
                    TempData.Remove("ReturnItems_DataTable_" + login.UserId);
                }
                catch (Exception ex)
                {

                }
            }




            MyOrdersDTO obj = new MyOrdersDTO();
            var PageSize = decimal.Parse(_options.PageSize);
            List<MyOrdersDTO> list = new List<MyOrdersDTO>();
            list = lstMyOrdersDTO;//(List<MyOrdersDTO>)TempData.Peek("TempHistoryOrderList");
            int toc = list != null && list.Count() > 0 ? list.Count : 0;
            int count = toc / Convert.ToInt32(PageSize);
            if (list != null && list.Count() > 0)
                list = pageNumber == 1 ? list.Take(Convert.ToInt32(PageSize)).ToList() : list.Skip(((pageNumber + Convert.ToInt32(PageSize)) - 2)).Take(Convert.ToInt32(PageSize)).ToList();
            obj.UsersProcessingOrdersList = list;
            obj.PageNumber = pageNumber;
            obj.pagingNumber = Convert.ToDecimal(Convert.ToDecimal(toc) / PageSize) > Convert.ToDecimal(Convert.ToInt32(Convert.ToDecimal(toc) / PageSize)) ? Convert.ToInt32((toc / PageSize)) + 1 : Convert.ToInt32((toc / PageSize));
            obj.UrlPath = apiurl + apiImages + UploadLocations.Products;
            //return PartialView("PVHistoryOrders", obj);
            //ViewBag.ProductImages = apiurl + apiImages + UploadLocations.Products;
            //try {
            //    var res = await  _viewRenderService.RenderToStringAsync("MyOrders/PVHistoryOrders", obj);
            //}
            //catch (Exception ex)
            //{ 
            
            //}
            var result = await _viewRenderService.RenderToStringAsync("MyOrders/PVHistoryOrders", obj);
            return Json(new { result = result });

        }
        public async Task<ActionResult> PartialAccept(Int64 orderId)
        {
            using (HttpClient client = new HttpClient())
            {
                bool status = false;
                _commonHeader.setHeaders(client);
                try
                {
                    MyOrdersDTO obj = new MyOrdersDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    if (login == null)
                    {
                        return RedirectToAction("Index", "User"); // later call login form
                    }
                    else
                    {
                        obj.UserId = login.UserId;
                    }


                    obj.OrderId = orderId;
                    obj.Message = "Partial Accept";

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewOrderPartialAccept", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        responseData = apiRes.SelectToken("Data").ToString();
                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                        if (status)
                        {
                            var resdata = apiRes.SelectToken("Data");
                            var orderjson = Convert.ToString(resdata.SelectToken("OrderJson"));
                            var currentOrder1 = JsonConvert.DeserializeObject<List<OrderDisplayDTO>>(orderjson.ToString());
                            var currentOrder = currentOrder1.FirstOrDefault();
                            if (currentOrder.OrderType == 3 && currentOrder.OrderStatus == 2)
                            {

                                StringBuilder sb = new StringBuilder();
                                string data = "";
                                int itemCount = 0;
                                decimal wholeweight = 0;
                                decimal weightKG = 0;
                                foreach (var item in currentOrder.Items.Where(x => x.IsAccepted == true))
                                {
                                    string variants = "";
                                    foreach (var varts in item.Variants)
                                    {
                                        variants += varts.OptionValueEn + "/";
                                    }
                                    if (!string.IsNullOrEmpty(variants))
                                        variants = variants.Remove(variants.Length - 1, 1);

                                    sb.Append(item.NameEn + "(" + variants + "), Qty:" + item.Qty + ", ");
                                    itemCount = itemCount + item.Qty;
                                    wholeweight = wholeweight + (item.Weight * item.Qty);
                                }
                                weightKG = wholeweight / 1000;   // weight in grams
                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    sb.Length--;
                                    sb.Length--;
                                }
                                if (sb.Length > 250)
                                    sb.ToString().Substring(0, 249);

                                string passKey = _options.SMSAPassKey;
                                SMSADTO smsaObj = new SMSADTO()
                                {
                                    passKey = passKey,
                                    refNo = currentOrder.InvoiceNo,
                                    sentDate = currentOrder.OrderDate.ToString(),
                                    idNo = currentOrder.InvoiceNo,
                                    cName = currentOrder.UserDetails.Select(x => x.FirstName).First(),

                                    Cntry = currentOrder.UserDetails.Select(x => x.CountryName).First(),
                                    cCity = currentOrder.UserDetails.Select(x => x.CityName).First(),

                                    cZip = currentOrder.UserDetails.Select(x => x.ZipCode).First(),
                                    cPOBox = "",
                                    cMobile = currentOrder.UserDetails.Select(x => x.MobileNo).First(),
                                    cTel1 = currentOrder.UserDetails.Select(x => x.Mobile2).First(),
                                    cTel2 = "",
                                    cAddr1 = currentOrder.UserDetails.Select(x => x.Address1).First(),
                                    cAddr2 = currentOrder.UserDetails.Select(x => x.Address2).First(),

                                    shipType = "DLV",
                                    PCs = itemCount,
                                    cEmail = currentOrder.UserDetails.Select(x => x.EmailId).First(),
                                    codAmt = currentOrder.PaymentMode == "COD" ? currentOrder.GrandTotal.ToString() : "0.00",
                                    weight = weightKG.ToString("N2"),
                                    itemDesc = sb.ToString(),
                                    prefDelvDate = currentOrder.ExpectingDelivery,
                                    carrValue = "",
                                    carrCurr = "",
                                    custCurr = "",
                                    custVal = "",
                                    insrCurr = "",
                                    insrAmt = "",

                                    sName = currentOrder.ShipperAddress.Select(x => x.FirstName).First(),
                                    sContact = currentOrder.ShipperAddress.Select(x => x.MobileNo).First(),
                                    sAddr1 = currentOrder.ShipperAddress.Select(x => x.Address1).First(),
                                    sAddr2 = "",
                                    sCity = currentOrder.ShipperAddress.Select(x => x.CityName).First(),
                                    sPhone = currentOrder.ShipperAddress.Select(x => x.MobileNo).First(),
                                    sCntry = currentOrder.ShipperAddress.Select(x => x.CountryName).First(),

                                    gpsPoints = "",
                                    awbNo = "",
                                    reason = ""
                                };
                                HttpResponseMessage responseMessage2 = await client.PostAsJsonAsync("api/SMSAAPI/AddShipment", smsaObj);
                                if (responseMessage2.IsSuccessStatusCode)
                                {
                                    var responseData2 = responseMessage2.Content.ReadAsStringAsync().Result;
                                    var response2 = JObject.Parse(responseData2);
                                    bool Status2 = Convert.ToBoolean(response2.SelectToken("Status"));
                                    var Message2 = Convert.ToString(response2.SelectToken("Message"));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    // AssignValuesToDTO.AssingDToValues(err, ex, "UserList/IndexGet", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    return RedirectToAction("Error");
                }
                return Json(status);
            }
        }
        public async Task<ActionResult> PartialReject(Int64 orderId)
        {
            using (HttpClient client = new HttpClient())
            {
                bool status = false;
                _commonHeader.setHeaders(client);
                try
                {
                    MyOrdersDTO obj = new MyOrdersDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    if (login == null)
                    {
                        return RedirectToAction("Index", "User"); // later call login form
                    }
                    else
                    {
                        obj.UserId = login.UserId;
                    }


                    obj.OrderId = orderId;
                    obj.Message = "Partial Reject";

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewOrderPartialReject", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        responseData = apiRes.SelectToken("Data").ToString();
                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    // AssignValuesToDTO.AssingDToValues(err, ex, "UserList/IndexGet", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    return RedirectToAction("Error");
                }
                return Json(status);
            }
        }
        public async Task<ActionResult> CancelOrder(Int64 orderId, int OrderType, int OrderStatus, string awbNo)
        {
            ResponseDTO resp = new ResponseDTO();
            using (HttpClient client = new HttpClient())
            {
                bool status = false;
                _commonHeader.setHeaders(client);
                try
                {
                    MyOrdersDTO obj = new MyOrdersDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    if (login == null)
                    {
                        return RedirectToAction("Index", "User"); // later call login form
                    }
                    else
                    {
                        obj.UserId = login.UserId;
                    }


                    obj.OrderId = orderId;
                    obj.Message = "Cancel Order";
                    obj.OrderType = OrderType;
                    obj.OrderStatus = OrderStatus;
                    obj.awbNo = awbNo;
                    if (obj.OrderType == 3 && obj.OrderStatus >= 2 && !string.IsNullOrEmpty(obj.awbNo))
                    {
                        obj.reason = "User cancel Order";
                        HttpResponseMessage responseMessage2 = await client.PostAsJsonAsync("api/SMSAAPI/cancelShipment", obj);
                        if (responseMessage2.IsSuccessStatusCode)
                        {
                            var responseData2 = responseMessage2.Content.ReadAsStringAsync().Result;
                            var response2 = JObject.Parse(responseData2);
                            bool Status2 = Convert.ToBoolean(response2.SelectToken("Status"));
                            var Message2 = Convert.ToString(response2.SelectToken("Message"));
                            if (Status2)
                            {
                                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewCancelOrder", obj);
                                if (responseMessage.IsSuccessStatusCode)
                                {
                                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                                    var apiRes = JObject.Parse(responseData);
                                    responseData = apiRes.SelectToken("Data").ToString();
                                    resp.Message = Convert.ToString(response2.SelectToken("Message"));
                                    resp.Status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                                    status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                                }
                            }
                            else

                            {
                                resp.Message = Message2;
                            }
                        }
                    }
                    else
                    {

                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewCancelOrder", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var apiRes = JObject.Parse(responseData);
                            responseData = apiRes.SelectToken("Data").ToString();
                            status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                            resp.Message = Convert.ToString(apiRes.SelectToken("Message"));
                            resp.Status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    // AssignValuesToDTO.AssingDToValues(err, ex, "UserList/IndexGet", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    return RedirectToAction("Error");
                }
                return Json(resp);
            }
        }

        [HttpPost]
        public async Task<PartialViewResult> ProcessingOrdersPV(int Type)
        {
            using (HttpClient client = new HttpClient())
            {
                bool status = false;
                _commonHeader.setHeaders(client);
                try
                {
                    UserHistoryByBranchTypeUserOrderIdDTO objNew = new UserHistoryByBranchTypeUserOrderIdDTO();
                    MyOrdersDTO obj = new MyOrdersDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    if (login == null)
                    {
                        return null; // later call login form
                    }
                    else
                    {
                        obj.Id = login.UserId;
                        objNew.Id = login.UserId;
                    }
                    //obj.Id = 13;
                    obj.Type = Type;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/UserHistoryOrdersByBranchTypeUserOrderId", objNew);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        var a = apiRes.SelectToken("Data").ToString();
                        //var b = JObject.Parse(a);
                        //var c = b.SelectToken("Result").ToString();


                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                        var lstUserPendingOrders = JsonConvert.DeserializeObject<List<MyOrdersDTO>>(a);
                        if (lstUserPendingOrders != null)
                        {
                            if (lstUserPendingOrders.Count != 0)
                                obj.UsersProcessingOrdersList = lstUserPendingOrders;
                            else
                                obj.UsersProcessingOrdersList = null;
                        }
                    }
                    return PartialView(obj);
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    //AssignValuesToDTO.AssingDToValues(err, ex, "UserList/IndexGet", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    return PartialView("Error");
                }
            }
        }
        public async Task<ActionResult> RescheduleAccept(Int64 orderId)
        {
            using (HttpClient client = new HttpClient())
            {
                bool status = false;
                _commonHeader.setHeaders(client);
                try
                {
                    MyOrdersDTO obj = new MyOrdersDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    if (login == null)
                    {
                        return RedirectToAction("Index", "User"); // later call login form
                    }
                    else
                    {
                        obj.UserId = login.UserId;
                    }


                    obj.OrderId = orderId;
                    obj.Message = "Reschedule Accept";

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewReScheduleAccept", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        responseData = apiRes.SelectToken("Data").ToString();
                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    // AssignValuesToDTO.AssingDToValues(err, ex, "UserList/IndexGet", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    return RedirectToAction("Error");
                }
                return Json(status);
            }
        }
        public async Task<ActionResult> RescheduleReject(Int64 orderId)
        {
            using (HttpClient client = new HttpClient())
            {
                bool status = false;
                _commonHeader.setHeaders(client);
                try
                {
                    MyOrdersDTO obj = new MyOrdersDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    if (login == null)
                    {
                        return null; // later call login form
                    }
                    else
                    {
                        obj.UserId = login.UserId;
                    }


                    obj.OrderId = orderId;
                    obj.Message = "Partial Reject";

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewReScheduleReject", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        responseData = apiRes.SelectToken("Data").ToString();
                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    // AssignValuesToDTO.AssingDToValues(err, ex, "UserList/IndexGet", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    return RedirectToAction("Error");
                }
                return Json(status);
            }
        }
        public async Task<ActionResult> UserReview(string SKUId, int Rating, string Comments, int BranchId, Int64 OrderID, int OrderType)
        {
            using (HttpClient client = new HttpClient())
            {
                bool status = false;
                _commonHeader.setHeaders(client);
                try
                {
                    MyOrdersDTO obj = new MyOrdersDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    if (login == null)
                    {
                        return RedirectToAction("Index", "User"); // later call login form
                    }
                    else
                    {
                        obj.UserId = login.UserId;
                    }

                    obj.Type = 2;
                    switch (OrderType)
                    {
                        case 1:
                        case 2:
                        case 3:
                            obj.StatusId = 1;
                            break;
                        case 4:
                        case 5:
                            obj.StatusId = 2;
                            break;
                        default:
                            break;
                    }
                    obj.Ids = SKUId;
                    obj.Rating = Rating;
                    obj.BranchId = BranchId;
                    obj.OrderId = OrderID;
                    obj.Comments = Comments;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewAddGetUserReviews", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        responseData = apiRes.SelectToken("Data").ToString();
                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    // AssignValuesToDTO.AssingDToValues(err, ex, "UserList/IndexGet", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    return RedirectToAction("Error");
                }
                return Json(status);
            }
        }



        public JsonResult AddReferencedocument(FormCollection form)
        {
            try
            {
                MyOrdersDTO obj = new MyOrdersDTO();
                SessionDTO login = AppUtils.UserLogin;
                if (login == null)
                    obj.UserId = 0;
                else
                    obj.UserId = login.UserId;

                DataTable dt = (DataTable)TempData.Peek("ReturnItems_DataTable_" + login.UserId);

                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("OrderId");
                    dt.Columns.Add("OrderItemId");
                    dt.Columns.Add("Image");
                    dt.Columns.Add("Qty");
                    dt.Columns.Add("FileName");

                    DataColumn workColumn = dt.Columns.Add("UniqueId", typeof(Int32));
                    workColumn.DataType = System.Type.GetType("System.Int32");
                    workColumn.AutoIncrement = true;
                    workColumn.AutoIncrementSeed = 1;
                    workColumn.AutoIncrementStep = 1;
                }

                string Qty = Request.Form["Qty"].ToString();
                string OrderId = Request.Form["OrderId"].ToString();
                string contentType = Request.Form["contentType"].ToString();
                string OrderItemId = Request.Form["OrderItemId"].ToString();

                DataRow[] drr = dt.Select("OrderItemId=" + OrderItemId);
                for (int g = 0; g < drr.Length; g++)
                    drr[g].Delete();
                dt.AcceptChanges();

                List<string> _s = new List<string>();
                //  Get all files from Request object  
                //////HttpFileCollectionBase files = Request.Files;
                //////for (int i = 0; i < files.Count; i++)
                //////{
                //////    MemoryStream target = new MemoryStream();
                //////    files[i].InputStream.CopyTo(target);

                //////    string thePictureDataAsString = Convert.ToBase64String(target.ToArray());
                //////    _s.Add(thePictureDataAsString);

                //////    DataRow _details = dt.NewRow();
                //////    _details["OrderId"] = OrderId;
                //////    _details["OrderItemId"] = OrderItemId;
                //////    _details["Qty"] = Qty;
                //////    _details["Image"] = thePictureDataAsString;
                //////    _details["FileName"] = files[i].FileName;

                //////    dt.Rows.Add(_details);
                //////}

                TempData["ReturnItems_DataTable_" + login.UserId] = dt;


                var x = (from r in dt.AsEnumerable()
                         select r["OrderId"]).Distinct().ToList();

                var y = (from r in dt.AsEnumerable()
                         select r["OrderItemId"]).Distinct().ToList();

                ReturnOrders_temp _gJson = new ReturnOrders_temp();
                List<ReturnOrders_temp> _glst = new List<ReturnOrders_temp>();

                for (int i = 0; i < x.Count(); i++)
                {
                    dynamic orderid = x[i];
                    _gJson.OrderId = long.Parse(orderid);
                    _gJson.UserId = login.UserId;
                    for (int k = 0; k < y.Count(); k++)
                    {
                        DataRow[] drrsa = dt.Select("OrderItemId=" + y[k]);
                        List<ReturnOrders_temp> _gimagelst = new List<ReturnOrders_temp>();
                        int Qty_s = 0;
                        foreach (DataRow row in drrsa)
                        {
                            Qty_s = int.Parse(row.Field<string>("Qty"));
                            string base64_Image_s = row.Field<string>("Image");
                            string FileName = row.Field<string>("FileName");
                            _gimagelst.Add(new ReturnOrders_temp { FileName = FileName, Base64FileData = base64_Image_s });
                        }
                        dynamic _orderitemid_dynamic = y[k];
                        long _orderitemid = long.Parse(_orderitemid_dynamic);
                        _glst.Add(new ReturnOrders_temp { OrderItemId = _orderitemid, Qty = Qty_s, Images = _gimagelst });
                    }
                    _gJson.JJItems = _glst;
                }

                //var jsonSerialiser = new JavaScriptSerializer();
                //var json = jsonSerialiser.Serialize(_gJson);
                var json = JsonConvert.SerializeObject(_gJson);

                return Json(json);
            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }

        public async Task<JsonResult> ReturnOrderRequest(FormCollection form)
        {
            using (HttpClient client = new HttpClient())
            {
                bool status = false;
                _commonHeader.setHeaders(client);
                try
                {
                    string Payment = Request.Form["Payment"].ToString();
                    MyOrdersDTO obj = new MyOrdersDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    if (login == null)
                        obj.UserId = 0;
                    else
                        obj.UserId = login.UserId;

                    DataTable dt = (DataTable)TempData.Peek("ReturnItems_DataTable_" + login.UserId);
                    if (dt == null)
                        return Json(false);

                    var x = (from r in dt.AsEnumerable()
                             select r["OrderId"]).Distinct().ToList();

                    var y = (from r in dt.AsEnumerable()
                             select r["OrderItemId"]).Distinct().ToList();

                    ReturnOrders_temp _gJson = new ReturnOrders_temp();
                    List<ReturnOrders_temp> _glst = new List<ReturnOrders_temp>();

                    for (int i = 0; i < x.Count(); i++)
                    {
                        dynamic orderid = x[i];
                        _gJson.OrderId = long.Parse(orderid);
                        _gJson.UserId = login.UserId;
                        for (int k = 0; k < y.Count(); k++)
                        {
                            DataRow[] drrsa = dt.Select("OrderItemId=" + y[k]);
                            List<ReturnOrders_temp> _gimagelst = new List<ReturnOrders_temp>();
                            int Qty_s = 0;
                            foreach (DataRow row in drrsa)
                            {
                                Qty_s = int.Parse(row.Field<string>("Qty"));
                                string base64_Image_s = row.Field<string>("Image");
                                string FileName = row.Field<string>("FileName");
                                _gimagelst.Add(new ReturnOrders_temp { FileName = FileName, Base64FileData = base64_Image_s });
                            }
                            dynamic _orderitemid_dynamic = y[k];
                            long _orderitemid = long.Parse(_orderitemid_dynamic);
                            _glst.Add(new ReturnOrders_temp { OrderItemId = _orderitemid, Qty = Qty_s, Images = _gimagelst });
                        }

                        _gJson.JJItems = _glst;
                        if (Payment == "COD")
                        {
                            _gJson.BankBIC = Request.Form["BankBIC"].ToString();
                            _gJson.AccountNo = Request.Form["AccountNumber"].ToString();
                            _gJson.FullName = Request.Form["AccountName"].ToString();
                            _gJson.IbanNo = Request.Form["IbanNumber"].ToString();
                        }
                    }

                    //var jsonSerialiser = new JavaScriptSerializer();
                    //_gJson.JItems = jsonSerialiser.Serialize(_gJson.JJItems);
                    _gJson.JItems = JsonConvert.SerializeObject(_gJson.JJItems);

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewInsertReturnRequest", _gJson);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        responseData = apiRes.SelectToken("Data").ToString();
                        status = Convert.ToBoolean(apiRes.SelectToken("Status").ToString());
                    }
                    //return Json(json);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
                return Json(status);
            }
        }
        public async Task<ActionResult> InvoiceDetails(string InvoiceNo)
        {
            string siteBaseURL = _options.baseUrl;
            ViewBag.RedirectionURL = siteBaseURL + "cart/OrderPaymentStatus";
            List<OrderDisplayDTO> ordersList = null;
            OrderDisplayDTO currentOrder = null;
            string api_url = apiurl;
            string upload_api = apiImages;
            string folder_name = UploadLocations.Products;

            ViewBag.UrlPath = api_url + upload_api + folder_name;

            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    OrderManagementDTO orderObj = new OrderManagementDTO();
                    orderObj.BranchId = 0;
                    orderObj.Type = 1;
                    orderObj.StatusId = 1;
                    //orderObj.InvoiceNo = Id;
                    orderObj.InvoiceNo = InvoiceNo;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/VendorOrderManagementAPI/NewGetOrderDetails", orderObj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        //var Data = response.SelectToken("Data");
                        var Data = (JArray)response["Data"];
                        if (Status)
                        {
                            var top1 = JsonConvert.DeserializeObject<List<OrderDisplayDTO>>(Data.ToString());
                            currentOrder = top1.FirstOrDefault();
                        }
                    }
                }
                catch (Exception Ex)
                {

                }
            }
            return View(currentOrder);
        }

        public async Task<ActionResult> OrderConfirmed(long Id)
        {
            string siteBaseURL = _options.baseUrl;
            List<OrderDisplayDTO> ordersList = null;
            OrderDisplayDTO currentOrder = null;
            string api_url = apiurl;
            string upload_api = apiImages;
            string folder_name = UploadLocations.Products;
            ViewBag.UrlPath = api_url + upload_api + folder_name;

            MyOrdersDTO orderObj = new MyOrdersDTO();
            orderObj.OrderId = Id;
            orderObj.Type = 1;

            SessionDTO login = AppUtils.UserLogin;
            if (login == null)
                return RedirectToAction("Index", "User"); // later call login form
            else
                orderObj.Id = login.UserId;

            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/VendorOrderManagementAPI/NewGetOrderDetails", orderObj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = (JArray)response["Data"];
                        var orders = JsonConvert.DeserializeObject<List<MyOrdersDTO>>(Data.ToString());
                        if (orders.Count != 0)
                            orderObj.UsersPendingOrdersList = orders;
                    }
                }
                catch (Exception Ex)
                {
                }
            }
            TempData.Remove("ReturnItems_DataTable_" + login.UserId);
            orderObj.UrlPath = api_url + upload_api + folder_name;
            return PartialView("OrderDetailsPV", orderObj);
        }


    }
}