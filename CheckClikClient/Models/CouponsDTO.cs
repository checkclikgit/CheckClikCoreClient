using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class CouponsDTO
    {
        public Int64 UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string VoucherNameEn { get; set; }
        public string VoucherNameAr { get; set; }
        public int VoucherDiscount { get; set; }
        public string CouponCode { get; set; }
        public string StoreNameEn { get; set; }
        public string StoreNameAr { get; set; }
        public IEnumerable<CouponsDTO> LstCoupons { get; set; }
        public long TotalRecords { get; set; }
        public int pagingNumber { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string UrlPath { get; set; }
        public string ApiURL { get; set; }
        public string LogoCopy { get; set; }
    }
}