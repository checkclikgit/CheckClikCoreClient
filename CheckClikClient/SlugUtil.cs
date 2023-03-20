using Customer.Models;
using Customer.Utils;
using System.Text.RegularExpressions;

namespace CheckClikClient
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
             
            string Brach = productListDto.BranchId.ToString();
            string data = String.Concat(StringUtil.URLEncrypt(productListDto.ProductNameEn + '_' + Brach + '_' + productListDto.Id + '_' + productListDto.BranchSubCategoryId));
            string phrase = string.Format("{0}", data); 
            string str = Regex.Replace(phrase, @"/", "-").Trim();

            return str;
        }

        public static string GenerateSlug(long Id, string ProductNameEn, string ProductSkuId, int ProductId, string UPCBarcode, int BranchId = 0)
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

        public static string GenerateSlugService(string ServiceNameEn, int BranchId, int ServiceId, int BranchSubCategoryId = 0)
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
            string Brach = productListDto.BranchId.ToString();
            string data = String.Concat(StringUtil.URLEncrypt(productListDto.ServiceNameEn + '_' + Brach + '_' + productListDto.ServiceId + '_' + productListDto.BranchSubCategoryId));
             
            string phrase = string.Format("{0}", data);
             
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
