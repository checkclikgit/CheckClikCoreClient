
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Customer.Models
{
    public class DealsListDTO
    {
     
            public string ProductSkuId { get; set; }
            public JObject Data { get; set; }
            public long Id { get; set; }
            public long UserId { get; set; }
            public int ProductId { get; set; }
            public string NameEn { get; set; }
            public string NameAr { get; set; }
            public string Image { get; set; }
            public string UPCBarcode { get; set; }
            public int DiscountType { get; set; }
            public Decimal DiscountValue { get; set; }
            public Decimal Price { get; set; }
            public Decimal SellingPrice { get; set; }
            public int StockQuantity { get; set; }
            public long TotalRecords { get; set; }
            public int SortId { get; set; }
            public int BranchSubCategoryId { get; set; }
            public int StoreId { get; set; }
            public int BranchId { get; set; }
            public int BranchCategoryId { get; set; }

            public int pagingNumber { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }

            public IEnumerable<DealsListDTO> DealsList { get; set; }
            public IEnumerable<DealsListDTO> ActionList { get; set; }
            public string UrlPath { get; set; }
            public int StoreType { get; set; }
            public long ServiceId { get; set; }
            public string ServiceNameEn { get; set; }
            public string ServiceNameAr { get; set; }
            public string CountingNameEn { get; set; }
            public string CountingNameAr { get; set; }
            public int SubCategoryId { get; set; }
            public bool Favorite { get; set; }
            public string ApiURL { get; set; }
            public string StoreNameEn { get;  set; }
            public int MainCatId { get; set; }
            public string MainCatEn { get; set; }
            public int SubCatId { get; set; }

            public string SubCatEn { get; set; }

        public DealsListDTO()
            {
                //this.ApiURL = System.Configuration.ConfigurationManager.AppSettings["apiurl"].ToString();
            }
            //public BaseViewModel bvm { get; set; }
            public string GenerateSlug()
            {
                long ticks = DateTime.Now.Ticks;
                byte[] bytes = BitConverter.GetBytes(ticks);
                string idss = Convert.ToBase64String(bytes)
                                        .Replace('+', '_')
                                        .Replace('/', '-')
                                        .TrimEnd('=');

            string Brach = BranchId.ToString();
            //string data = String.Concat(NameEn + '_' + Brach + '_' + Id+'_' + BranchSubCategoryId);

            //string phrase = string.Format("{0}-{1}-{2}-{3}-{4}", data, ProductSkuId, idss, ProductId, UPCBarcode);

            //string str = RemoveAccent(phrase).ToLower();
            //    // invalid chars           
            //    str = Regex.Replace(str, @"[^a-z0-9\s-_]", "");
            //    // convert multiple spaces into one space   
            //    str = Regex.Replace(str, @"\s+", " ").Trim();
            //    // cut and trim 
            //    str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            //    str = Regex.Replace(str, @"\s", "-"); // hyphens   
            //string data = String.Concat(StringUtil.URLEncrypt(NameEn.Replace(" ", "-") + '_' + Brach + '_' + Id + '_' + BranchSubCategoryId));
            string data = "";
            //string phrase = string.Format("{0}-{1}-{2}-{3}", data, ProductSkuId, idss, ProductId, UPCBarcode);
            string phrase = string.Format("{0}", data);

            //string str = RemoveAccent(phrase).ToLower();
            //// invalid chars           
            //str = Regex.Replace(str, @"[^a-z0-9\s-_]", "");
            //// convert multiple spaces into one space   
            //str = Regex.Replace(str, @"\s+", " ").Trim();
            //// cut and trim 
            //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            //str = Regex.Replace(str, @"\s", "-"); // hyphens   
            string str = Regex.Replace(phrase, @"/", "-").Trim();
            return str;
            }
        public string GenerateSlugService()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            string Brach = BranchId.ToString();
            //string data = String.Concat(ServiceNameEn + '_' + Brach + '_' + ServiceId + '_' + BranchSubCategoryId);

            ////string phrase = string.Format("{0}-{1}-{2}", ServiceId, ServiceNameEn,CountingNameEn);
            //string phrase = string.Format("{0}-{1}", data, CountingNameEn);

            //string str = RemoveAccent(phrase).ToLower();
            //// invalid chars           
            //str = Regex.Replace(str, @"[^a-z0-9\s-_]", "");
            //// convert multiple spaces into one space   
            //str = Regex.Replace(str, @"\s+", " ").Trim();
            //// cut and trim 
            //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            //str = Regex.Replace(str, @"\s", "-"); // hyphens   
            string data = "";//String.Concat(StringUtil.URLEncrypt(ServiceNameEn + '_' + Brach + '_' + ServiceId + '_' + BranchSubCategoryId));

            //string phrase = string.Format("{0}-{1}", data, CountingNameEn);
            string phrase = string.Format("{0}", data);

            //string str = RemoveAccent(phrase).ToLower();
            //// invalid chars           
            //str = Regex.Replace(str, @"[^a-z0-9\s-_]", "");
            //// convert multiple spaces into one space   
            //str = Regex.Replace(str, @"\s+", " ").Trim();
            //// cut and trim 
            //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            //str = Regex.Replace(str, @"\s", "-"); // hyphens   
            string str = Regex.Replace(phrase, @"/", "-").Trim();
            return str;
        }

        private string RemoveAccent(string text)
            {
                byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
                return System.Text.Encoding.ASCII.GetString(bytes);
            }
        }
   
}