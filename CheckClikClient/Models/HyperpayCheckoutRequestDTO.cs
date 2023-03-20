using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class HyperpayCheckoutRequestDTO
    {
        public string amount { get; set; }
        public string shopperResultUrl { get; set; }
        public Boolean isCardRegistration { get; set; }
        public string merchantTransactionId { get; set; }
        public string customerEmail { get; set; }
        public string customerPhone { get; set; } = string.Empty;
        public string customerName { get; set; } = string.Empty;
        public string userId { get; set; }
        public string resourcePath { get; set; }
        public string registrationId { get; set; }
        public string CheckoutId { get; set; }
        public Int64 OrderId { get; set; }
        public bool isApplePay { get; set; } = false;
        public bool isMadaCard { get; set; } = false;
        public bool isVisaMaster { get; set; } = true;
        public int paymentBy { get; set; } = 1; // 1- Customer; 2- Vendor

        public string Street1 { get; set; } = "Olaya";
        public string City { get; set; } = "Riyadh";
        public string State { get; set; } = "Riyadh";
        public string PostCode { get; set; } = "50033";
        public string Country { get; set; } = "SA";

    }
}