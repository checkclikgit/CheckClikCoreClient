using SearchLibrary.Models;
using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchLibrary
{
    public class ProductSearchResult
    {
        [SolrUniqueKey]
        public string ProductSKUId { get; set; }

        [SolrField]
        public string ProductId { get; set; }

        [SolrField]
        public string ItemType { get; set; }

        [SolrField]
        public int ProductBranchId { get; set; }

        [SolrField]
        public string ProductTypeBranchId { get; set; }

        [SolrField]
        public string[] TagsAr { get; set; }

        [SolrField]
        public string MetaDescriptionAr { get; set; }

        [SolrField]
        public string DescriptionEn { get; set; }

        [SolrField]
        public string NameEn { get; set; }

        [SolrField]
        public string[] TagsEn { get; set; }

        [SolrField]
        public string MetaDescriptionEn { get; set; }

        [SolrField]
        public string DescriptionAr { get; set; }

        [SolrField]
        public string NameAr { get; set; }
        [SolrField]
        public int BranchId { get; set; }

        [SolrField]
        public string BranchNameEn { get; set; }

        [SolrField]
        public string BranchNameAr { get; set; }

        [SolrField]
        public string ProductMainImage { get; set; }

        [SolrField]
        public double SellingPrice { get; set; }
        [SolrField]
        public int DiscountType { get; set; }
        [SolrField]
        public double DiscountValue { get; set; }

        [SolrField]
        public double Price { get; set; }

        [SolrField]
        public bool InStock { get; set; }

        [SolrField]
        public int StoreId { get; set; }

        [SolrField]
        public string StoreNameEn { get; set; }

        [SolrField]
        public string StoreNameAr { get; set; }

        [SolrField]
        public decimal? DistToStoreBranch { get; set; }

        [SolrField]
        public string UPCBarcode { get; set; }

        [SolrField]
        public int? ServiceType { get; set; }

        [SolrField]
        public decimal? score { get; set; }        

        [SolrField("*")]
        public IDictionary<string, object> OtherFields { get; set; }

        public long StoreItemsCount { get; set; }

        public StaticFacetsDTO StaticFacets { get; set; }
        public string FacetAr { get; set; }
        //public int MyProperty { get; set; }

        //kamrul
        public string StoreLogo { get; set; }
        //kamrul
        public string StoreBackground { get; set; }

        //kamrul
        public string ReviewsCount { get; set; }
        //kamrul
        public string Ratings { get; set; }


        public int StoreType { get; set; }

        public String ContactNo { get; set; }
        public String ZipCode { get; set; }
        public String StreetNo { get; set; }
        public String BuildingNo { get; set; }
        public String FloorNo { get; set; }
        public String ApartmentOfficeNo { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
    }
}