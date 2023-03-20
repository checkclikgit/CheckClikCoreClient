using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchLibrary
{
    public class Product
    {
        public string ProductId { get; set; }

        public string[] TagsAr { get; set; }

        public string MetaDescriptionAr { get; set; }

        public string DescriptionEn { get; set; }

        public string NameEn { get; set; }

        public string[] TagsEn { get; set; }

        public string MetaDescriptionEn { get; set; }

        public string DescriptionAr { get; set; }

        public string NameAr { get; set; }

    }
}