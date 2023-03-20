
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.Models
{
    public class StaticFacetsDTO
    {
        public bool IsPickup { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsShipping { get; set; }
        public int? ServiceType { get; set; }        
        public bool IsCOD { get; set; }
        public bool IsCCPN { get; set; }
        public bool IsCCPL { get; set; }
        public bool IsDCPN { get; set; }
        public bool IsDCPL { get; set; }
        public string ItemType { get; set; }
        public string Price { get; set; }

        public decimal MinPrice
        {
            get
            {
                return !(string.IsNullOrWhiteSpace(Price) || Price.ToLower() == "undefined") ? Convert.ToDecimal(Price.Split(',')[0]) : 0;
            }
        }
        public decimal MaxPrice
        {
            get
            {
                return !(string.IsNullOrWhiteSpace(Price) || Price.ToLower() == "undefined") ? Convert.ToDecimal(Price.Split(',')[1]) : 0;
            }
        }
    }
}
