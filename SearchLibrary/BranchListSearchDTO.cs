using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary
{
    public class BranchListSearchDTO
    {
        public int StoreId { get; set; }
        public int BranchId { get; set; }
        public string StoreNameEn { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
