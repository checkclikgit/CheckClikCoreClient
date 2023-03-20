﻿using CheckClickClient.Models;
using CheckClikClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class BranchDTO 
    {
        public int Id { get; set; } = 0;
        public int StoreId { get; set; } = 0;
        public string BranchNameEn { get; set; } = string.Empty;
        public string BranchNameAr { get; set; } = string.Empty;
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
        public int ProductCount { get; set; }
        //public List<BranchCommunicationsDTO> Communications { get; set; }
        public int BranchSubCategoryId { get; set; }


        public List<MainCategoryDTO> listCategory { get; set; }
        public List<SubCategoryDTO> listSubCategory { get; set; }
        public List<BranchDTO> listBranch { get; set; }
        public List<BannersDTO> listBanner { get; set; }
        public BaseViewModel bvm { get; set; }
        public string ChatData { get; set; }
        public string ApiURL { get; set; }
        public int StoreType { get; set; }
        public string ReturnConditions { get; set; }
        public string NoFaultStore { get; set; }
        public string NonReturnableProduct { get; set; }

        public BranchDTO()
        {
            //this.ApiURL = System.Configuration.ConfigurationManager.AppSettings["apiurl"].ToString();
        }

  

    }
}