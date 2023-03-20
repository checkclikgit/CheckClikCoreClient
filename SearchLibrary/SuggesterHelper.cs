using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary
{
    public class SuggestionHelper
    {
        private readonly IConfiguration _config;

        public SuggestionHelper(IConfiguration config)
        {
            _config = config;
        }
        public async Task<List<string>> Suggest(string query, string language,string branchId ="0")
        {
            try
            {
                string strMainURL = _config["SolrURL"];
                //string strMainURL = ConfigurationManager.AppSettings["SolrURL"];
                string solrUrl = strMainURL + "products";
                string mySuggesterName = language == "ar" ? "mySuggesterAr" : "mySuggester";
                string relativeUrl = "/suggest";

                var parameters = new Dictionary<string, string>
                {
                    {"q", query},
                    {"wt", "json"},
                    {"suggest.dictionary", mySuggesterName }
                };
                
                //if(branchId != "0" && branchId != "" && branchId != null)
                //{
                //    parameters.Add("cfq", branchId);
                //    parameters.Add("suggest.dictionary", "mySuggesterBranch");
                //}
                //else
                //{
                //    parameters.Add("suggest.dictionary", mySuggesterName);
                //}

                var solrConnection = new SolrConnection(solrUrl);
                string response = await solrConnection.GetAsync(relativeUrl, parameters);
                JObject jsonVal = JObject.Parse(response);
                JProperty jPropSuggestions = jsonVal.SelectToken("suggest").SelectToken(mySuggesterName).First().ToObject<JProperty>();
                int iSearchResultCounts = Convert.ToInt32(jPropSuggestions.Value.SelectToken("numFound"));
                JArray arSearchResults = (JArray)jPropSuggestions.Value.SelectToken("suggestions");

                List<string> lstSearchResults = new List<string>();
                for (int i = 0; i < arSearchResults.Count; i++)
                {
                    lstSearchResults.Add(arSearchResults[i].SelectToken("term").Value<string>());
                }
                lstSearchResults = lstSearchResults.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();

                return lstSearchResults;
            }
            catch(Exception exc)
            {
                //return null;
                List<string> lst = new List<string>();
                lst.Add(exc.Message);
                return lst;
            }
        }

    }
}
