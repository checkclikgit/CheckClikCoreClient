using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.Models
{
    public class SolrSearchQuery
    {
        public SolrSearchQuery()
        {
            Rows = 10;
            Start = 0;
            Facets = new List<SearchFacetsDTO>();
            StaticFacets = new StaticFacetsDTO();
            Language = "en";
        }
        public string Query { get; set; }
        public int Start { get; set; }
        public int Rows { get; set; }
        public string Location { get; set; }
        public string Language { get; set; }
        public List<SearchFacetsDTO> Facets { get; set; }
        public StaticFacetsDTO StaticFacets { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public string BranchId { get; set; } = "";
        //kamrul
        public string CatId { get; set; } = "";
        //kamrul
        public string SubCatId { get; set; } = "";
    }
}
