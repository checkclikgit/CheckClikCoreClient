using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer
{
    public class MessageOTPDTO
    {
        public int Id { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Message { get; set; }
        //public string Reply { get; set; }
        public string OTPCode { get; set; }
        public DateTime ValidUntil { get; set; }

        public long FlagId { get; set; }

        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }
}