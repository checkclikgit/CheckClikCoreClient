using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
   
    public class HomePageSliderDTO
    {
        public int Id { get; set; }
        public string CatType { get; set; }
        public int Position { get; set; }
        public string Language { get; set; }
        public string ImageEn { get; set; }


        public string HeaderText { get; set; }
        public string Description { get; set; }
        public string HeaderTextP { get; set; }

        public string DescriptionP { get; set; }

        public string HeaderTextC { get; set; }

        public string DescriptionC { get; set; }

        public int HiddenPositionID { get; set; }
        public string HiddenCatType { get; set; }
        public bool Status { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public long DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public long FlagId { get; set; }
        public IEnumerable<HomePageSliderDTO> HomePageSliderList { get; set; }
        //public List<string> biImageEnf { get; set; }


    }
}