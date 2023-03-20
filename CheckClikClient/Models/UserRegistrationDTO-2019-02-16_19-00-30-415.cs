using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer
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
        
        public int FlagId { get; set; }
    }
    public class CustomerRegistrationDTO
    {

        public Int64 Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Reply { get; set; }
        public string OTPCode { get; set; }
        public string MobileNo { get; set; }
        public string Message { get; set; }
        public string EmailId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ProfilePhoto { get; set; }
        //public HttpPostedFileBase ProfilePhoto1 { get; set; }
        public string Address { get; set; }
        public string StatusCode { get; set; }
        public string Language { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string DeviceVersion { get; set; }
        public byte[] PhotoBytes { get; set; }
        public string StatusMessage { get; set; }
        public DateTime ValidUntil { get; set; }
        //public int MyProperty { get; set; }

        public bool IsActive { get; set; }
        public long FlagId { get; set; }
    }
}