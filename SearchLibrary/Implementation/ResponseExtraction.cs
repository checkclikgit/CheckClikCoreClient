using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchLibrary.Models;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;

namespace SearchLibrary.Implementation
{
    class ResponseExtraction
    {
        internal void SetHeader<T>(QueryResponse<T> queryResponse, SolrQueryResults<T> solrResults)
        {
            queryResponse.QueryTime = solrResults.Header.QTime;
            queryResponse.Status = solrResults.Header.Status;
            queryResponse.NumFound = (int)solrResults.NumFound;

            if (solrResults.Grouping.Count > 0)
                queryResponse.NGroups = Convert.ToInt32(solrResults.Grouping.FirstOrDefault().Value.Ngroups);
            else
                queryResponse.NGroups = 0;
        }

        internal void SetBody<T>(QueryResponse<T> queryResponse, SolrQueryResults<T> solrResults)
        {
            queryResponse.Results = solrResults;
        }

        internal void SetSpellCheck<T>(QueryResponse<T> queryResponse, SolrQueryResults<T> solrResults)
        {
            List<string> lstSpellSuggestions = new List<string>();

            foreach (SpellCheckResult spellResult in solrResults.SpellChecking)
            {
                foreach (string suggestion in spellResult.Suggestions)
                {
                    lstSpellSuggestions.Add(suggestion);
                }
            }
            queryResponse.DidYouMean = lstSpellSuggestions;
        }

        internal void SetFacets<T>(QueryResponse<T> queryResponse, SolrQueryResults<T> solrResults)
        {
            List<FacetsResponse> lstFacetsResponse = new List<FacetsResponse>();

            if (queryResponse.OriginalQuery.Facets.Count < solrResults.FacetFields.Count)
            {
                return;
            }

            List<SearchFacetsDTO> lstQueryFacets = queryResponse.OriginalQuery.Facets.ToList();
            var facetFields = solrResults.FacetFields.Where(f => f.Key.Contains("#") || f.Key.Equals("ManufacturerEn") || f.Key.Equals("ItemType") || f.Key.Equals("ServiceType")).ToList();

            foreach (var solrFacet in facetFields)
            {
                string strFieldText = string.Empty;
                int facetId = 0;
                if (solrFacet.Key.Contains("#"))
                {
                    strFieldText = solrFacet.Key.Split(new char[] { '#' })[0];
                    string strSubText = solrFacet.Key.Substring(solrFacet.Key.IndexOf("#") + 1);
                    facetId = Convert.ToInt32(strSubText.Substring(0, strSubText.IndexOf("_facet")));
                }
                else
                {
                    strFieldText = solrFacet.Key;
                }

                FacetsResponse facetsResponse = new FacetsResponse
                {
                    FacetEn = strFieldText,
                    Id = facetId
                };
                SearchFacetsDTO currentSearchFacet = queryResponse.OriginalQuery.Facets.Where(fa => fa.SelectedFacets.Count > 0 && fa.Id == facetId).FirstOrDefault();
                int iFacetTextId = 1;
                foreach (var facetFilter in solrResults.FacetFields[solrFacet.Key])
                {
                    bool isSearchFilter = false;
                    if (currentSearchFacet != null)
                        isSearchFilter = currentSearchFacet.SelectedFacets.Any(s => s.Equals(facetFilter.Key));
                    else if (strFieldText == "ItemType")
                    {
                        isSearchFilter = queryResponse.OriginalQuery.StaticFacets.ItemType == facetFilter.Key;
                    }
                    else if (strFieldText == "ServiceType")
                    {
                        if (!string.IsNullOrWhiteSpace(facetFilter.Key))
                        {
                            isSearchFilter = queryResponse.OriginalQuery.StaticFacets.ServiceType.ToString() == facetFilter.Key;
                        }
                    }

                    facetsResponse.FacetFilters.Add(new FacetItem { FacetTextId = iFacetTextId, FacetText = facetFilter.Key, Count = facetFilter.Value, Selected = isSearchFilter });
                    iFacetTextId++;
                }

                lstFacetsResponse.Add(facetsResponse);
            }
            List<FacetsResponse> lstFacetsFiltered = lstFacetsResponse.Where(fr => fr.FacetFilters.Count > 0).OrderByDescending(fr => fr.FacetFilters.Count).ToList();
            //if(query.StaticFacets.IsCOD)
            //    lstFacetsFiltered.Add(new FacetsResponse { FacetEn = "IsCOD", Id = 0, FacetFilters.Add(  })

            queryResponse.Facets = lstFacetsFiltered;
        }

        internal void SetStats<T>(QueryResponse<T> queryResponse, SolrQueryResults<T> solrResults)
        {
            foreach (var kv in solrResults.Stats)
            {
                if (kv.Key == "Price")
                {
                    if (kv.Value.Min.ToString() != "NaN")
                        queryResponse.MinPrice = kv.Value.Min;

                    if (kv.Value.Max.ToString() != "NaN")
                        queryResponse.MaxPrice = kv.Value.Max;
                }
                else if (kv.Key == "ProductBranchId")
                {
                    foreach (var f in kv.Value.FacetResults)
                    {
                        foreach (var fv in f.Value)
                        {
                            T currentResult = FindFirstResult(solrResults, fv.Key);
                            currentResult?.GetType().GetProperty("StoreItemsCount").SetValue(currentResult, fv.Value.Count);
                        }
                    }
                }
            }
        }

        private T FindFirstResult<T>(SolrQueryResults<T> solrResults, string strStoreId)
        {
            if (int.TryParse(strStoreId, out int iStoreId))
            {
                return solrResults.FirstOrDefault(x =>
                {
                    var dic = x as dynamic;
                    var propertyInfo = dic.GetType().GetProperty("StoreId");
                    var val = propertyInfo.GetValue(dic, null);
                    return iStoreId == val;
                });
            }
            return solrResults.ElementAtOrDefault(0);
        }
    }
}
