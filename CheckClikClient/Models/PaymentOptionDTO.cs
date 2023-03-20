using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class PaymentOptionDTO
    {

        public int CountryId { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }
        public int BranchId { get; set; }
        public bool IsCashAllowed { get; set; }
        public bool IsCreditCardAllowed { get; set; }
        public bool IsCcPayNow { get; set; }
        public bool IsCcPayLater { get; set; }
        public bool IsMadaCardAllowed { get; set; }

        public bool IsApplePayAllowed { get; set; }

        //public bool IsDebitCardAllowed { get; set; }
        //public bool IsDcPayNow { get; set; }
        //public bool IsDcPayLater { get; set; }
        public long CreatedBy { get; set; }
        public bool IsPayment { get; set; }
        public int OrderType { get; set; }
        public bool IsShipment { get; set; }
        //public Nullable<DateTime> ExpectedDate { get; set; }
        public string ExpectedDate { get; set; }
        public bool Status { get; set; }
        public int Type { get; set; }
    }
}