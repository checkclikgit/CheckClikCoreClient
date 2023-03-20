using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.Models
{
    public class QueryResponse<T>
    {
        public QueryResponse()
        {
            StaticFacets = new StaticFacetsDTO();
        }
        public List<T> Results { get; set; }
        public int NumFound { get; set; }
        public int NGroups { get; set; }
        public int QueryTime { get; set; }
        public int Status { get; set; }
        public SolrSearchQuery OriginalQuery { get; set; }

        public List<string> DidYouMean { get; set; }

        public List<FacetsResponse> Facets { get; set; }
        public StaticFacetsDTO StaticFacets { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string OriginalPriceRange { get; set; }
        public int OriginalResultsCount { get; set; }
    }
}
