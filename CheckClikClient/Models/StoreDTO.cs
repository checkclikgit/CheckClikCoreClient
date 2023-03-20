using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Customer.Models
{
    public class StoreDTO
    {
        public Int64 BranchId { get; set; }
        public string BranchEn { get; set; }
        public string BranchAr { get; set; }
        public string MinimumOrderValue { get; set; }
        public string StoreId { get; set; }
        public string StoreEn { get; set; }
        public string StoreAr { get; set; }
        public int CategoryId { get; set; }
        public string CategoryEn { get; set; }
        public string CategoryAr { get; set; }
        public Int64 StoreCount { get; set; }

        public bool IsCashAllowed { get; set; }
        public bool IsCreditCardAllowed { get; set; }
        public string DeliveryTime { get; set; }
        public string Reviews { get; set; }
      
        public int ReviewsCount { get; set; }
        public string BranchStatus { get; set; }
        public string OrderStatus { get; set; }
        public int PaymentType { get; set; }
        public int Ratings { get; set; }
        public float Distance { get; set; }


        public string BackgroundImage {get;set;}
        public string BranchLogoImage {get;set;}
        public string BranchColor     {get;set;}
        public string StoreImage { get; set; }
        public string Background { get; set; }
        public string Logo { get; set; }
        public string ApiURL { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string mapdata { get; set; }
        public string Address { get; set; }
        public int VendorType { get; set; }
        public int Type { get; set; }
        public int StoreType { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long VisitCount { get; set; }

        public List<StoreDTO> list { get; set; }
        public string GenerateSlugMainCat()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');
            string Brach = BranchId.ToString();
            string data = String.Concat(StoreEn.Replace(" ","-") + '_' + Brach + '_' + StoreId);

            //string phrase = string.Format("{0}-{1}-{2}", ServiceId, ServiceNameEn,CountingNameEn);
            string phrase = string.Format("{0}", data);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-_]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
        public StoreDTO()
        {
            //this.ApiURL = System.Configuration.ConfigurationManager.AppSettings["apiurl"].ToString();

        }

    }
}