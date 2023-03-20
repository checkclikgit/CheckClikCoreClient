using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace CheckClickClient.Models
{
    public class CustomerRegistrationDTO
    {
        public Int64 Id { get; set; }
        public Int64 OrderId { get; set; }
        public Int64 UserId { get; set; }
        
        public JObject Data { get; set; }
        public int BranchId { get; set; }
        public int StoreId { get; set; }
        public string StoreNameEn { get; set; }
        public string BranchNameEn { get; set; }
        public string StoreNameAr { get; set; }
        public string InvoiceNo { get; set; }
        public int ItemsCount { get; set; }
        public decimal GrandTotal { get; set; }
        public int Type { get; set; }
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
        public string PhotoBytes { get; set; }
        public string Address { get; set; }
        public string AddressAr { get; set; }
        public string StatusCode { get; set; }
        public string Language { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string DeviceVersion { get; set; }
        public int PageNumber { get; set; } //startRowIndex
        public int PageSize { get; set; } //maximumRows
        public long TotalRecords { get; set; }
        public int pagingNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string StatusMessage { get; set; }
        public DateTime ValidUntil { get; set; }
        public IEnumerable<CustomerRegistrationDTO> UsersList { get; set; }
        public IEnumerable<CustomerRegistrationDTO> UsersOrderHistoryList { get; set; }
        public IEnumerable<CustomerRegistrationDTO> ActionList { get; set; }
        public string UrlPath { get; set; }
        public string SearchText { get; set; }
        public int Blocked { get; set; }
        public int ActionBy { get; set; }
        public string BlockedReason { get; set; }
        //public int MyProperty { get; set; }

        public bool IsActive { get; set; }
        public bool IsTerminated { get; set; }
        public long FlagId { get; set; }
        public string CountryName { get;set;}
        public string CityName { get; set; }
        public string Mobile2 { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string BranchAddress { get; set; }

    }
    public class CustomerEmailSubscriptionDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string EmailId { get; set; }
        public bool Status { get; set; }
        public long StatusCode { get; set; }
        public string Message { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public long DeletedBy { get; set; }
        public long FlagId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SearchText { get; set; }
        public IEnumerable<CustomerEmailSubscriptionDTO> EmailSubList { get; set; }
        public long RowRank { get; set; }
        public DateTime CreatedDate { get; set; }
        public long TotalRecords { get; set; }
        public int pagingNumber { get; set; }
    }
}