using Newtonsoft.Json.Linq;
using SearchLibrary.Implementation;
using SearchLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary
{
    public class ProductSearch : IProductSearch
    {
        private Search<ProductSearchResult> _search;
        private SuggestionHelper _suggestionHelper;
        public ProductSearch(SuggestionHelper suggestionHelper)
        {
            _search = new Search<ProductSearchResult>();
            _suggestionHelper = suggestionHelper;
        }
        public async Task<QueryResponse<ProductSearchResult>> Search(SolrSearchQuery solrSearchQuery)
        {
            if (!solrSearchQuery.Query.EndsWith("*"))
                solrSearchQuery.Query += "*";

            QueryResponse<ProductSearchResult> searchResults = await _search.DoSearch(solrSearchQuery);
            return searchResults;
        }

        public async Task<List<string>> Suggest(string query, string language,string branchId)
        {
            //SuggestionHelper suggester = new SuggestionHelper();
            return await _suggestionHelper.Suggest(query, language,branchId);
            //Search<ProductSearchResult> search = new Search<ProductSearchResult>("products");
            //return await search.Suggest(query);
        }
    }


    public interface IProductSearch
    {
        Task<QueryResponse<ProductSearchResult>> Search(SolrSearchQuery solrSearchQuery);
        Task<List<string>> Suggest(string query, string language,string branchId);
    }
}
