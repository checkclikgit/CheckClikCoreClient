using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary
{
    public class SearchFacetsDTO
    {
        public int Id { get; set; }
        public string FacetsEn { get; set; }
        public string FacetsAr { get; set; }
        public List<string> SelectedFacets { get; set; }
        public SearchFacetsDTO()
        {
            SelectedFacets = new List<string>();
        }
    }
}
