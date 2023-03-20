using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class CustomerManageAddressDTO 
    {
        public long Id { get; set; }
        public long UserId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Address1")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string Address1 { get; set; }

        [Required]
        [Display(Name = "Address2")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "Select Country")]
        public long CountryId { get; set; }

        [Required]
        [Display(Name = "Select City")]
        public long CityId { get; set; }


        [Required(ErrorMessage = "Zip Code is Required")]
        [Display(Name = "Zipcode")]
        [StringLength(50, ErrorMessage = "Must be Under 6 characters")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage ="Phone Number is Required")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(9, ErrorMessage = "Must be Under 9 characters")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Select City")]
        public long AddressType { get; set; }
        public bool Status { get; set; }
        public long StatusCode { get; set; }
        public string Message { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public long DeletedBy { get; set; }
        public long FlagId { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public string CountryNameEn { get; set; }
        public string CountryNameAr { get; set; }
        public string CityNameEn { get; set; }
        public string CityNameAr { get; set; }
        public IEnumerable<CountryDTO> CountryList { get; set; }
        public IEnumerable<CitysDTO> CityList { get; set; }
        public IEnumerable<CustomerManageAddressDTO> ListOfCustomerAddress { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string LastName { get; set; }
        //public string Reply { get; set; }
        public string OTPCode { get; set; }
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "*Enter Email Id", AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Enter Correct Email Address")]
        public string EmailId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ProfilePhoto { get; set; }
        //public HttpPostedFileBase ProfilePhoto1 { get; set; }
        public string Address { get; set; }
        public string Language { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string DeviceVersion { get; set; }
        //public byte[] PhotoBytes { get; set; }
        public string PhotoBytes { get; set; }
        public string StatusMessage { get; set; }
        public DateTime ValidUntil { get; set; }
        //public int MyProperty { get; set; }

        public bool IsActive { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        
        public string Mobile2 { get; set; }
        public string BranchAddress { get; set; }
    }
    public class CustomerManageAddressArDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Address1")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string Address1 { get; set; }

        [Required]
        [Display(Name = "Address2")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "إختر دولة")]
        public long CountryId { get; set; }

        [Required]
        [Display(Name = "إختر مدينة")]
        public long CityId { get; set; }


        [Required(ErrorMessage = "Zip Code is Required")]
        [Display(Name = "Zipcode")]
        [StringLength(50, ErrorMessage = "Must be Under 6 characters")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "Phone Number is Required")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(9, ErrorMessage = "Must be Under 9 characters")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Select City")]
        public long AddressType { get; set; }
        public bool Status { get; set; }
        public long StatusCode { get; set; }
        public string Message { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public long DeletedBy { get; set; }
        public long FlagId { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public string CountryNameEn { get; set; }
        public string CountryNameAr { get; set; }
        public string CityNameEn { get; set; }
        public string CityNameAr { get; set; }
        public IEnumerable<CountryDTO> CountryList { get; set; }
        public IEnumerable<CitysDTO> CityList { get; set; }
        public IEnumerable<CustomerManageAddressDTO> ListOfCustomerAddress { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string LastName { get; set; }
        //public string Reply { get; set; }
        public string OTPCode { get; set; }
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "*Enter Email Id", AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Enter Correct Email Address")]
        public string EmailId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ProfilePhoto { get; set; }
        //public HttpPostedFileBase ProfilePhoto1 { get; set; }
        public string Address { get; set; }
        public string Language { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string DeviceVersion { get; set; }
        //public byte[] PhotoBytes { get; set; }
        public string PhotoBytes { get; set; }
        public string StatusMessage { get; set; }
        public DateTime ValidUntil { get; set; }
        //public int MyProperty { get; set; }

        public bool IsActive { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }

        public string Mobile2 { get; set; }
        public string BranchAddress { get; set; }
    }
    public class CountryDTO
    {
        public long Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string CodeEn { get; set; }
        public string CurrencyCode { get; set; }
        //public long Id { get; set; }
        //public string NameEn { get; set; }
        //public string NameAr { get; set; }
        //public string CodeEn { get; set; }
        //public string Icon { get; set; }
        //public string CurrencyCode { get; set; }
        //public bool Status { get; set; }
        //public long CreatedBy { get; set; }
        //public long ModifiedBy { get; set; }
        //public long DeletedBy { get; set; }
        //public bool IsDeleted { get; set; }

        //public long FlagId { get; set; }

        //public string StatusCode { get; set; }
        //public string StatusMessage { get; set; }
    }
    public class CitysDTO
    {
        public long Id { get; set; }
        public long CountryID { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Latitude { get; set; }
        public string Langitude { get; set; }
    }
}