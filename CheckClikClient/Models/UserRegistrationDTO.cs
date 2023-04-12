using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using System.Web.Mvc;

namespace CheckClickClient.Models
{
    public class UserRegistrationDTO
    {
        public int Id { get; set; }
        public Int64 UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtpCode { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ProfilePhoto { get; set; }
        public int FlagId { get; set; }
        public string UserChatId { get; set; }
        public string? CRN { get; set; }
        public JArray JGroupChatId { get; set; }
    }
    public class CustomerRegistrationDTO
    {

        public Int64 Id { get; set; }

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
        public string Message { get; set; }

        [Required(ErrorMessage = "*Enter Email Id", AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Enter Correct Email Address")]
        public string EmailId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ProfilePhoto { get; set; }
        public IFormFile ProfilePhoto1 { get; set; }
        public string Address { get; set; }
        public string StatusCode { get; set; }
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
        public long FlagId { get; set; }
        public bool Status { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "Address1")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string Address1 { get; set; }

        [Required]
        [Display(Name = "Address2")]
        [StringLength(50, ErrorMessage = "Must be Under 50 characters")]
        public string Address2 { get; set; }
        public string Mobile2 { get; set; }
        public string BranchAddress { get; set; }

    }
    public class CustomerEmailSubscriptionDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string EmailId { get; set; }
        public bool Status { get; set; }
        public long StatusCode { get; set; }
        public string Message { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public long DeletedBy { get; set; }
        public long FlagId { get; set; }
    }
}