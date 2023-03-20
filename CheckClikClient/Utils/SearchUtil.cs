using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Utils
{
    public static class SearchUtil
    {
        public static int PageSize { get; set; }
        static SearchUtil()
        {
            PageSize = 12;
        }
    }
}