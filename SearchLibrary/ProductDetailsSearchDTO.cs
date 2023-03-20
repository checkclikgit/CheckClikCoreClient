using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary
{
    public class ProductDetailsSearchDTO
    {
        public long UserId { get; set; }
        public long ProductInventoryId { get; set; }
        public int BranchSubCategoryId { get; set; }
    }
}
