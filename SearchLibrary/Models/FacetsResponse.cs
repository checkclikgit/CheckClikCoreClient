using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.Models
{
    public class FacetsResponse
    {
        public int Id { get; set; }
        public string FacetEn { get; set; }
        public string FacetAr { get; set; }
        public List<FacetItem> FacetFilters { get; set; }
        public FacetsResponse()
        {
            FacetFilters = new List<FacetItem>();
        }
    }
    public class FacetItem
    {
        public int FacetTextId { get; set; }
        public string FacetText { get; set; }
        public int Count { get; set; }
        public bool Selected { get; set; }
    }

}
