using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public Int64 OrderId { get; set; }
        public string MessageEn { get; set; }
        public string MessageAr { get; set; }
        public int Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentDate { get; set; }

        public List<NotificationDTO> listNotification { get; set; }
        public int PageNumber { get; set; }
        public int TotalRecords { get; set; }
        public int pagingNumber { get; set; }
        public int PageSize { get; set; }
        public int Unreadcount { get; set; }
    }
}