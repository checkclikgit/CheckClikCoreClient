using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class BannersDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int OfferType { get; set; }
        public string OfferTypeName { get; set; }
        ////public HttpPostedFileBase BannerImageBaseEn { get; set; }
        public string BannerImageEn { get; set; }

        public string BannerImageAr { get; set; }
        public int Percentage { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public string[] RegionId { get; set; }
        public string RegionIds { get; set; }
        public bool IsUptoApplicable { get; set; }
        public int WeekNo { get; set; }
        public int StoreId { get; set; }
        public int BranchId { get; set; }
        public string[] StoreArr { get; set; }
        public string StoreNameEn { get; set; }
        public string StoreNameAr { get; set; }
        public string VendorTypeName { get; set; }

        public string StoreTypeEn { get; set; }
        public string MainCategories { get; set; }


        public int Position { get; set; }
        public string strRegionId { get; set; }
        public bool Status { get; set; }
        public long ActionBy { get; set; }

        public bool IsDeleted { get; set; }

        public long FlagId { get; set; }

        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string OfferStatus { get; set; }
        public string[] OfferStatusArr { get; set; }
        public string StoreIds { get; set; }
        public int UserType { get; set; }
        public string ApiURL { get; set; }
        public int MainCatId { get; set; }
        public string MainCatEn { get; set; }
        public string MainCatAr { get; set; }
        public string SubCatEn { get; set; }
        public string SubCatAr { get; set; }
        public int InvId { get; set; }
        public int BranchSubCategoryId { get; set; }
        
        public BannersDTO()
        {
            ////this.ApiURL = System.Configuration.ConfigurationManager.AppSettings["apiurl"].ToString();

        }

    }
}