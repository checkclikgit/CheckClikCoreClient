using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Distance { get; set; }
        public int Type { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

    }

    public class DistrictDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string CityEn { get; set; }
        public int CityId { get; set; }
        public string Distance { get; set; }

    }
}