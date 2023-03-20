using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class AutocompleteDTO
    {
        public List<string> AutocompleteList { get; set; }
        public bool Status { get; set; }
        public long StatusCode { get; set; }
    }
}