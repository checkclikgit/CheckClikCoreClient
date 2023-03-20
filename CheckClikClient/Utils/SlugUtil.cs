using Customer.Models;
using Customer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Customer
{
    public class SlugUtil
    {
        public static string GenerateSlug(ProductListDTO productListDto)
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            //string phrase = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", productListDto.Id, productListDto.ProductNameEn, productListDto.ProductSkuId, idss, productListDto.ProductId, productListDto.UPCBarcode);

            string Brach = productListDto.BranchId.ToString();
            string data = String.Concat(StringUtil.URLEncrypt(productListDto.ProductNameEn + '_' + Brach + '_' + productListDto.Id + '_' + productListDto.BranchSubCategoryId));
            string phrase = string.Format("{0}", data);
            //string phrase = string.Format("{0}-{1}-{2}-{3}", data, productListDto.ProductSkuId, idss, productListDto.ProductId, productListDto.UPCBarcode);

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

        public static string GenerateSlug(long Id, string ProductNameEn, string ProductSkuId, int ProductId, string UPCBarcode,int BranchId=0)
        {
            ProductListDTO productListDto = new ProductListDTO
            {
                Id = Id,
                ProductNameEn = ProductNameEn,
                ProductSkuId = ProductSkuId,
                ProductId = ProductId,
                UPCBarcode = UPCBarcode,
                BranchId = BranchId
                
            };

            return GenerateSlug(productListDto);
        }

        public static string GenerateSlugService(string ServiceNameEn, int BranchId, int ServiceId,int BranchSubCategoryId=0)
        {
            ProductListDTO productListDto = new ProductListDTO
            {
                ServiceNameEn = ServiceNameEn,
                BranchId = BranchId,
                ServiceId = ServiceId,
                BranchSubCategoryId = BranchSubCategoryId
            };

            return GenerateSlugService(productListDto);
        }

        public static string GenerateSlugService(ProductListDTO productListDto)
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            //string phrase = string.Format("{0}-{1}-{2}", productListDto.ServiceId, productListDto.ServiceNameEn, productListDto.CountingNameEn);

            string Brach = productListDto.BranchId.ToString();
            string data = String.Concat(StringUtil.URLEncrypt(productListDto.ServiceNameEn + '_' + Brach + '_' + productListDto.ServiceId + '_' + productListDto.BranchSubCategoryId));

            //string phrase = string.Format("{0}-{1}", data, productListDto.CountingNameEn);
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

        private static string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}