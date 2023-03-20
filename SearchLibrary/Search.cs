using Newtonsoft.Json.Linq;
using SearchLibrary.Implementation;
using SearchLibrary.Models;
using SearchLibrary.Utils;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SearchLibrary
{
    public class Search<T>
    {
        //private Connection<T> connection;
        //public Search(string module)
        //{
        //    //connection = new Connection<T>(module);
        //}

        public async Task<QueryResponse<T>> DoSearch(SolrSearchQuery query)
        {
            try
            {
                SolrQueryResults<T> solrResults;
                QueryResponse<T> queryResponse = new QueryResponse<T>
                {
                    OriginalQuery = query
                };
                Connection<T> connection = new Connection<T>();
                ISolrOperations<T> solr = connection.GetSolrInstance();
                solr.BuildSpellCheckDictionary();

                QueryBuilder solrQueryBuilder = new QueryBuilder();

                //if(HttpContext.Current?.Session["searchString"] != null)
                //if(HttpContext.Current.Session["searchString"] = query;

                StatsParameters stats = new StatsParameters
                {
                    Facets = new[] { "StoreId" }                   
                };
                stats.AddField("ProductBranchId");
                // fq added
               // stats.AddField("appends");

                stats.AddField("{!min=true max=true}Price");

                QueryOptions queryOptions = new QueryOptions
                {
                    Rows = query.Rows,
                    StartOrCursor = new StartOrCursor.Start(query.Start * query.Rows),
                    SpellCheck = new SpellCheckingParameters { Collate = true },
                    Grouping = new GroupingParameters()
                    {
                      // Fields = new[] { "StoreNameEn", "NameEn" },
                        Format = GroupingFormat.Simple,
                        //Offset = 1,
                        //Limit = 2,
                        //Limit = 1,
                        Ngroups = true
                    },
                    Facet = solrQueryBuilder.BuildFacets(query),
                    //FilterQueries = new ISolrQuery[] { new SolrQueryByRange<decimal>("Price", query.StaticFacets.MinPrice, query.StaticFacets.MaxPrice) },
                    //FilterQueries = solrQueryBuilder.BuildFilterQueries(query),
                    //Fields = solrQueryBuilder.BuildFields(query),
                    Stats = stats,
                    ExtraParams = solrQueryBuilder.BuildExtraParams(query),                   
                };

                // Added by naveed
                if (!string.IsNullOrEmpty(query.BranchId) && query.BranchId != "0")
                {
                    queryOptions.FilterQueries = new ISolrQuery[] { new SolrQueryByField("BranchId", query.BranchId) };
                    queryOptions.Grouping.Fields = new[] { "NameEn" }; 
                }
                else
                {
                    queryOptions.Grouping.Fields = new[] { "StoreNameEn", "NameEn" }; 
                }


                //queryOptions.OrderBy = new[] { new SortOrder("geodist()", Order.ASC), SortOrder.Parse("id asc") };
                //queryOptions.OrderBy = new[] { new SortOrder("score", Order.DESC) };
                //queryOptions.OrderBy = new[] { SortOrder.Parse("score desc"), SortOrder.Parse("Price asc") };
                if (!string.IsNullOrWhiteSpace(query.SortField))
                {
                    if (query.SortField != "Relevence")
                        queryOptions.OrderBy = new[] { SortOrder.Parse(query.SortField + " " + query.SortDirection) };
                }

                ISolrQuery solrQuery = new SolrQuery(query.Query);
                solrResults = await solr.QueryAsync(solrQuery, queryOptions);


               // solrResults.Where(x=>x ["BranchId"] == query.BranchId)

                ResponseExtraction extractResponse = new ResponseExtraction();
                extractResponse.SetHeader(queryResponse, solrResults);
                extractResponse.SetBody(queryResponse, solrResults);
                extractResponse.SetSpellCheck(queryResponse, solrResults);
                extractResponse.SetFacets(queryResponse, solrResults);
                extractResponse.SetStats(queryResponse, solrResults);
                queryResponse.StaticFacets = query.StaticFacets;

                return queryResponse;
            }
            catch (Exception exc)
            {
                ExceptionUtility.LogException(exc, "SearchAPI -> Search");
            }
            return null;
        }
    }
}
