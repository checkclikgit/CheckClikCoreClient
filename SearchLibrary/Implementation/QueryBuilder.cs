using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchLibrary.Models;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace SearchLibrary.Implementation
{
    class QueryBuilder
    {
        internal FacetParameters BuildFacets(SolrSearchQuery query)
        {
            FacetParameters facetParameters = new FacetParameters();
            List<ISolrFacetQuery> lstFacetFields = new List<ISolrFacetQuery>();
            for (int i = 0; i < query.Facets.Count; i++)
            {
                if (query.Facets[i].Id == 0)
                {
                    if (!(query.Facets[i].FacetsEn == "ManufacturerEn" || query.Facets[i].FacetsEn == "ItemType" || query.Facets[i].FacetsEn == "ServiceType"))
                        continue;
                }

                lstFacetFields.Add(new SolrFacetFieldQuery(string.Concat("{!ex=excludeTags}", query.Facets[i].FacetsEn)));
            }
            lstFacetFields.Add(new SolrFacetFieldQuery(string.Concat("{!ex=excludeTags}", "Price")));
            facetParameters.Queries = lstFacetFields.ToArray();
            facetParameters.MinCount = 1;
            facetParameters.Sort = true;

            return facetParameters;
        }

        //internal ICollection<ISolrQuery> BuildFilterQueries(SolrSearchQuery query)
        //{
        //    ICollection<ISolrQuery> lstFacetByFields = new List<ISolrQuery>();
        //    for (int i = 0; i < query.Facets.Count; i++)
        //    {
        //        if (query.Facets[i].Id == 0)
        //        {
        //            if (query.Facets[i].FacetsEn != "ManufacturerEn")
        //                lstFacetByFields.Add(new SolrQueryByField(string.Concat("{!ex=excludeTags}", query.Facets[i].FacetsEn), "true"));
        //        }
        //    }

        //    return lstFacetByFields;
        //}

        private List<KeyValuePair<string, string>> BuildFilters(SolrSearchQuery query)
        {
            List<KeyValuePair<string, string>> lstFilters = new List<KeyValuePair<string, string>>();

            List<SearchFacetsDTO> lstSearchFacets = query.Facets.Where(f => f.SelectedFacets.Count > 0).ToList();

            for (int i = 0; i < lstSearchFacets.Count; i++)
            {
                StringBuilder sbFacetFilters = new StringBuilder();
                for (int j = 0; j < lstSearchFacets[i].SelectedFacets.Count; j++)
                    sbFacetFilters.Append(lstSearchFacets[i].SelectedFacets[j]).Append(" OR ");

                sbFacetFilters = sbFacetFilters.Remove(sbFacetFilters.ToString().LastIndexOf(" OR "), 4);

                lstFilters.Add(new KeyValuePair<string, string>("fq", string.Concat("{!tag=excludeTags}", lstSearchFacets[i].FacetsEn, ":", sbFacetFilters.ToString())));
            }
            bool isMatched = false;
            for (int i = 0; i < query.Facets.Count; i++)
            {
                switch (query.Facets[i].FacetsEn)
                {
                    case "IsPickup":
                        if (query.StaticFacets.IsPickup)
                            isMatched = true;
                        break;
                    case "IsDelivery":
                        if (query.StaticFacets.IsDelivery)
                            isMatched = true;
                        break;
                    case "IsShipping":
                        if (query.StaticFacets.IsShipping)
                            isMatched = true;
                        break;
                    case "IsCOD":
                        if (query.StaticFacets.IsCOD)
                            isMatched = true;
                        break;
                    case "IsCCPN":
                        if (query.StaticFacets.IsCCPN)
                            isMatched = true;
                        break;
                    case "IsCCPL":
                        if (query.StaticFacets.IsCCPL)
                            isMatched = true;
                        break;
                    case "IsDCPN":
                        if (query.StaticFacets.IsDCPN)
                            isMatched = true;
                        break;
                    case "IsDCPL":
                        if (query.StaticFacets.IsDCPL)
                            isMatched = true;
                        break;
                    case "ItemType":
                        if (!string.IsNullOrWhiteSpace(query.StaticFacets.ItemType))
                            lstFilters.Add(new KeyValuePair<string, string>("fq", string.Concat("{!tag=excludeTags}", query.Facets[i].FacetsEn, ":", query.StaticFacets.ItemType)));
                        continue;
                    case "ServiceType":
                        if (!(query.StaticFacets.ServiceType==null || query.StaticFacets.ServiceType == 0))
                            lstFilters.Add(new KeyValuePair<string, string>("fq", string.Concat("{!tag=excludeTags}", query.Facets[i].FacetsEn, ":", query.StaticFacets.ServiceType)));
                        continue;
                    //case "Price":
                    //    lstFilters.Add(new KeyValuePair<string, string>("fq", string.Concat("{!tag=excludeTags}", query.Facets[i].FacetsEn, ":[", query.StaticFacets.MinPrice," TO ",query.StaticFacets.MaxPrice, "]")));
                    //    continue;
                    default:
                        continue;
                }

                if (isMatched)
                    lstFilters.Add(new KeyValuePair<string, string>("fq", string.Concat(query.Facets[i].FacetsEn, ":true")));
            }

            if (query.StaticFacets.MaxPrice != 0)
                lstFilters.Add(new KeyValuePair<string, string>("fq", string.Concat("{!tag=excludeTags}", "Price", ":[", query.StaticFacets.MinPrice, " TO ", query.StaticFacets.MaxPrice, "]")));

            return lstFilters;
        }

        internal List<KeyValuePair<string, string>> BuildExtraParams(SolrSearchQuery query)
        {
            string strSearchQuery = query.Query;
            if (query.Query.EndsWith("*"))
                strSearchQuery = query.Query.Remove(query.Query.LastIndexOf("*"));

            List<KeyValuePair<string, string>> lstExtraParams = BuildFilters(query);
            lstExtraParams.Add(new KeyValuePair<string, string>("group.sort", "score desc,geodist() asc"));
            lstExtraParams.Add(new KeyValuePair<string, string>("pt", query.Location));
            lstExtraParams.Add(new KeyValuePair<string, string>("sfield", "location"));

           

            //Add Arabic Columns Start
            if (query.Language == "ar")
            {
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "NameAr:\"" + strSearchQuery + "\"^100"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "DescriptionAr:\"" + strSearchQuery + "\"^50"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "ManufacturerAr:\"" + strSearchQuery + "\"^10"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "TagsAr:\"" + strSearchQuery + "\"^10"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "MetaDescriptionAr:\"" + strSearchQuery + "\"^10"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "StoreNameAr:\"" + strSearchQuery + "\"^5"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "BranchNameAr:\"" + strSearchQuery + "\"^5"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "BranchSubCategoryAr:\"" + strSearchQuery + "\"^5"));
            }
            else
            {
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "NameEn:\"" + strSearchQuery + "\"^100"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "DescriptionEn:\"" + strSearchQuery + "\"^50"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "ManufacturerEn:\"" + strSearchQuery + "\"^10"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "TagsEn:\"" + strSearchQuery + "\"^10"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "MetaDescriptionEn:\"" + strSearchQuery + "\"^10"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "StoreNameEn:\"" + strSearchQuery + "\"^5"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "BranchNameEn:\"" + strSearchQuery + "\"^5"));
                lstExtraParams.Add(new KeyValuePair<string, string>("bq", "BranchSubCategoryEn:\"" + strSearchQuery + "\"^5"));
                
            }
            //Add Arabic Columns End
            lstExtraParams.Add(new KeyValuePair<string, string>("bq", "UPCBarcode:\"" + strSearchQuery + "\"^100"));
            lstExtraParams.Add(new KeyValuePair<string, string>("bq", "InStock:true^10"));
            lstExtraParams.Add(new KeyValuePair<string, string>("bq", "InStock:false^1"));
            lstExtraParams.Add(new KeyValuePair<string, string>("bf", "recip(geodist(),1,100,100)^10"));
            lstExtraParams.Add(new KeyValuePair<string, string>("bf", "recip(Price,1,100,100)^10"));
            lstExtraParams.Add(new KeyValuePair<string, string>("bf", "recip(ms(NOW,CreatedOn),3.16e-11,1,1)^10"));
            //lstExtraParams.Add(new KeyValuePair<string, string>("fq", "Price:[0 TO 50]"));
            return lstExtraParams;
        }

        internal List<string> BuildFields(SolrSearchQuery query)
        {
            List<string> lstFields = new List<string>
            {
                "ProductBranchId",
                "ProductSKUId",
                "BranchId",
                "ItemType",
                "ProductId",
                "SellingPrice",
                "Price",
                "DiscountType",
                "SkuId",
                "DiscountValue",
                "UPCBarcode",
                "ProductMainImage",
                "InStock",
                "StoreId",
                "DistToStoreBranch:geodist()",
                "score"
            };

            if (query.Language == "ar")
            {
                lstFields.Add("NameAr");
                lstFields.Add("DescriptionAr");
                lstFields.Add("TagsAr");
                lstFields.Add("MetaDescriptionAr");
                lstFields.Add("ManufacturerAr");
                lstFields.Add("StorArameAr");
                lstFields.Add("BranchNameAr");
                lstFields.Add("BranchSubCategoryAr");
            }
            else
            {
                lstFields.Add("NameEn");
                lstFields.Add("DescriptionEn");
                lstFields.Add("TagsEn");
                lstFields.Add("MetaDescriptionEn");
                lstFields.Add("ManufacturerEn");
                lstFields.Add("StoreNameEn");
                lstFields.Add("BranchNameEn");
                lstFields.Add("BranchSubCategoryEn");
            }
            
            return lstFields;
        }

        internal List<KeyValuePair<string, string>> BuildFilterQuery(SolrSearchQuery query)
        {


            List<KeyValuePair<string, string>> lstExtraParams = BuildFilters(query);

            lstExtraParams.Add(new KeyValuePair<string, string>("fq", "BranchId:" + 9));

   
        
            return lstExtraParams;
        }
    }
}
