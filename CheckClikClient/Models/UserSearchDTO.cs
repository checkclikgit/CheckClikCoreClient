using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class UserSearchDTO
    {
        public string txtSearchTerm { get; set; } = "";
        public string hdnlocation { get; set; } = "";
        public string hdnIsLocationSet { get; set; } = "";
        public string hdnSearchText { get; set; } = "";
        public string hdnSearchFilters { get; set; } = "";
        public int hdnStart { get; set; } = 0;
        public string hdnSort { get; set; } = ""; 
         
    }
}