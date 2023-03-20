using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int StatusId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Rating { get; set; }
        public string ProfilePhoto { get; set; }
        public string FirstName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Comments { get; set; }
        public int BranchId{ get; set; }
        
        public List<ReviewDTO> list { get; set; }
        public int ReviewCount { get; set; }
        public int pagingNumber { get;  set; }
        public string ApiUrl { get; set; }
    }
}