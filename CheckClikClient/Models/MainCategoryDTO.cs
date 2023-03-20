//using Customer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CheckClickClient.Models
{
    public class MainCategoryDTO
    {
        public long Id { get; set; }
        public int BranchId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int CategoryTypeId { get; set; }
        public int ProductCount { get; set; }
        public string IconClass { get; set; }
        public string MainCagtegoryFamilyEn { get; set; }
        public string MainCagtegoryFamilyAr { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public long DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
      

        public long FlagId { get; set; }
        public IEnumerable<MainCategoryDTO> LstMainCategories { get; set; }
        //public IEnumerable<MainCategoryFamilyDTO> LstMainCategoryFamily { get; set; }
        public IEnumerable<MainCategoryDTO> LstfaICONS { get; set; }

        public string GenerateSlug()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            //string phrase = string.Format("{0}%{1}", Id, NameEn);

            string data = "";//String.Concat(StringUtil.URLEncrypt(NameEn +'-'+ BranchId.ToString() + '-' + Id.ToString()));
            //string data = String.Concat(StringUtil.Crypt(BranchId.ToString()) + '-' + StringUtil.Crypt(Id.ToString())+ '-' + StringUtil.Crypt("")) ;
            //string data = String.Concat(StringUtil.Crypt(BranchId.ToString() + '-' + Id.ToString() + ""));
            //string data = String.Concat(StringCipher.URLEncrypt(NameEn) + '-' + StringCipher.URLEncrypt(BranchId.ToString()) + '-' + StringCipher.URLEncrypt(Id.ToString()));

            //string phrase = string.Format("{0}-{1}-{2}", ServiceId, ServiceNameEn,CountingNameEn);
            string phrase = string.Format("{0}", data);
            //string data = string.Concat(NameEn + '-' + Id);
            //string phrase = string.Format("{0}", data);

            //string str = RemoveAccent(phrase).ToLower();
            ////string str = phrase;
            //// invalid chars           
            //str = Regex.Replace(str, @"[^a-z0-9\s-_]", "");
            //// convert multiple spaces into one space   
           string str = Regex.Replace(phrase, @"/", "-").Trim();
            //// cut and trim 
            ////str = str.Substring(0, str.Length <= 64 ? str.Length : 63).Trim();
            //str = Regex.Replace(str, @"\s", "_"); // hyphens   
            return str;
        }
        public string GenerateSlugAR()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            //string phrase = string.Format("{0}%{1}", Id, NameEn);

            string data = "";//String.Concat(StringUtil.URLEncrypt(NameAr +'-'+ BranchId.ToString() + '-' + Id.ToString()));
            //string data = String.Concat(StringUtil.Crypt(BranchId.ToString()) + '-' + StringUtil.Crypt(Id.ToString())+ '-' + StringUtil.Crypt("")) ;
            //string data = String.Concat(StringUtil.Crypt(BranchId.ToString() + '-' + Id.ToString() + ""));
            //string data = String.Concat(StringCipher.URLEncrypt(NameEn) + '-' + StringCipher.URLEncrypt(BranchId.ToString()) + '-' + StringCipher.URLEncrypt(Id.ToString()));

            //string phrase = string.Format("{0}-{1}-{2}", ServiceId, ServiceNameEn,CountingNameEn);
            string phrase = string.Format("{0}", data);
            //string data = string.Concat(NameEn + '-' + Id);
            //string phrase = string.Format("{0}", data);

            //string str = RemoveAccent(phrase).ToLower();
            ////string str = phrase;
            //// invalid chars           
            //str = Regex.Replace(str, @"[^a-z0-9\s-_]", "");
            //// convert multiple spaces into one space   
           string str = Regex.Replace(phrase, @"/", "-").Trim();
            //// cut and trim 
            ////str = str.Substring(0, str.Length <= 64 ? str.Length : 63).Trim();
            //str = Regex.Replace(str, @"\s", "_"); // hyphens   
            return str;
        }
        public string GenerateSlugStore()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            //string phrase = string.Format("{0}%{1}", Id, NameEn);
            //string data = string.Concat(NameEn + '-' + Id);
            string phrase = string.Format("{0}", NameEn);

            string str = RemoveAccent(phrase).ToLower();
            //string str = phrase;
            // invalid chars           
            //str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
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
        public int CType { get; set; }
        // public long SubCategoryId { get; set; }
    }
}