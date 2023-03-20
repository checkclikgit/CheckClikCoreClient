using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class PNDeviceTokenDTO
    {
        public Int64 Id { get; set; }
        public long UserId { get; set; }
        public long UserType { get; set; }
        public string TopicARN { get; set; }
        public string SubscriptionARN { get; set; }
        public string EndPoint { get; set; }
        public long FlagId { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string jARN { get; set; }
        public bool Status { get; set; }
        public string ChatId { get; set; }
        public string GroupChatId { get; set; }
        public int StoreId { get; set; }
        public int BranchId { get; set; }
    }
}