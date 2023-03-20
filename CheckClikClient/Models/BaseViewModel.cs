//using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class BaseViewModel
    {
        public int Id { get; set; } = 0;
        public int StoreId { get; set; } = 0;
        public string BranchNameEn { get; set; } = string.Empty;
        public string BranchNameAr { get; set; } = string.Empty;
        public string StoreNameEn { get; set; } = string.Empty;
        public string StoreEn { get; set; } = string.Empty;
        public string StoreNameAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public int CountryId { get; set; } = 0;
        public int CityId { get; set; } = 0;
        public int DistrictId { get; set; } = 0;
        public string ZipCode { get; set; } = string.Empty;
        public string StreetNo { get; set; } = string.Empty;
        public string BuildingNo { get; set; } = string.Empty;
        public string FloorNo { get; set; } = string.Empty;
        public string ApartmentOfficeNo { get; set; } = string.Empty;
        public int AddressTypeId { get; set; } = 0;
        public DateTime BusinessSince { get; set; } = DateTime.Now.AddYears(-100);
        public bool Pickup { get; set; } = false;
        public bool InStoreService { get; set; } = false;
        public int PreBookingInStoreTimeUnit { get; set; } = 0;
        public int PreBookingInStoreTime { get; set; } = 0;
        public bool IsDelivery { get; set; } = false;
        public int IsDeliveryCompany { get; set; } = 1;
        public int PreBookingDeliveryTimeUnit { get; set; } = 0;
        public int PreBookingDeliveryTime { get; set; } = 0;
        public string DeliveryTime { get; set; } 
        public double DeliveryCharges { get; set; } = 0;
        public double MinimumOrderValue { get; set; } = 0;
        public double FreeDeliveryAboveValue { get; set; } = 0;
        public double TransportationCharges { get; set; } = 0;
        public bool IsShippingWorldwide { get; set; } = false;
        public int ShippingProvider { get; set; } = 0;
        public string TermsAndConditionsEn { get; set; } = string.Empty;
        public string TermsAndConditionsAr { get; set; } = string.Empty;
        public string ReturnPolicyEn { get; set; } = string.Empty;
        public string ReturnPolicyAr { get; set; } = string.Empty;
        //public HttpPostedFileBase BackgroundImageFile { get; set; }
        public string BackgroundImage { get; set; }
        //public HttpPostedFileBase BranchLogoImageFile { get; set; }
        public string BranchLogoImage { get; set; }

        public string BranchColor { get; set; } = string.Empty;
        public int MembershipPlan { get; set; } = 0;
        public bool PlanStatus { get; set; } = false;
        public bool AvailabilityStatus { get; set; } = false;
        public string QRCode { get; set; } = string.Empty;
        public string Facebook { get; set; } = string.Empty;
        public string TwitterHandle { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string Snapchat { get; set; } = string.Empty;
        public string Youtube { get; set; } = string.Empty;
        public string Maroof { get; set; } = string.Empty;
        //public DataTable branchCommunications { get; set; }

        public bool Status { get; set; } = true;
        public long CreatedBy { get; set; } = 0;
        public long ModifiedBy { get; set; } = 0;
        public long DeletedBy { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public long FlagId { get; set; } = 0;
        public string StatusCode { get; set; } = string.Empty;
        public string StatusMessage { get; set; } = string.Empty;
        public string BranchShiftTimingsJson { get; set; }
        public string message { get; set; }
        //public List<BranchCommunicationsDTO> Communications { get; set; }
        public string BranchStatus { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string OrderDone { get; set; }
        public string Shift { get; set; }
        public string Background { get; set; }
        public string Logo { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public string ApiURL { get; set; }
        public int VendorType { get; set; }
        public int Ratings { get; set; }
        public int ReviewsCount { get; set; }
		public string ChatData { get; set; }
        public int StoreType { get; set; }
        public string OtherBranches { get; set; }
        public bool Favorite1 { get; set; }
        public string EncId { get;set; }
        public long VisitCount { get; set; }
        public string strBranchShift { get; set; }
        public List<ShiftTimingDTO> BranchShift { get; set; }
        public int ReturnDays { get; set; }
        public string Address { get; set; }
        public string CountryEn { get; set; }
        public string RegionEn { get; set; }
        public string CityEn { get; set; }
        public string DistrictEn { get; set; }

        public string[] ReturnPolicyArArr { get; set; }
        public string[] NoFaultStoreArr { get; set; }
        public string[] NonReturnableProductArr { get; set; }
        public BaseViewModel()
        {
            //this.ApiURL = System.Configuration.ConfigurationManager.AppSettings["apiurl"].ToString();
        }

    }
}
