using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer
{
    public class ResponseDTO
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public long Id { get; set; }
        public string ValidTime { get; set; }
        public string ReturnUrl { get; set; }
        public string GroupChatId { get; set; }
    }
}