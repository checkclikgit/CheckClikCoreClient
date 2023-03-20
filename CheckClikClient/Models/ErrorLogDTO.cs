using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class ErrorLogDTO
    {
        public string userId { get; set; }
        public string ExcType { get; set; }
        public string ExcMessage { get; set; }
        public string ExcSource { get; set; }
        public string ExcStackTrace { get; set; }
        public string pageUrl { get; set; }
        public string methodName { get; set; }
        public int LineNo { get; set; }
        public string timezone { get; set; }
    }
}