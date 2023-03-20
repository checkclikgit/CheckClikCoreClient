using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class OrderTrackingDTO
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 UserId { get; set; }
    }
}