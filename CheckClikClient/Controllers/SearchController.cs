using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SearchLibrary.Models;
using SearchLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using CheckClikClient.Models;
using CheckClickClient;

namespace CheckClikClient.Controllers
{

    public class SearchController : Controller
    {

        string currentUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;
        private readonly IViewRenderService _viewRenderService;

        public SearchController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options.Value;
            var baseURL = _options.baseUrl;// System.Configuration.ConfigurationManager.AppSettings["baseUrl"].ToString().ToLower();
            currentUrl = _httpContextAccessor.HttpContext.Request.PathBase.ToString();//System.Web.HttpContext.Current.Request.Url.ToString().ToLower();// Current.Request.Url.AbsolutePath;
            currentUrl = currentUrl.Replace(baseURL, baseURL + "ar/");
            ViewBag.CurrentURL = currentUrl;
            _errorHandler = errorHandler;
            _commonHeader = commonHeader;
            _viewRenderService = viewRenderService;

        }
        //[Route("search",Order =1)]
        [HttpGet]
        [Route("search/query")]
        public async Task<ActionResult> Index(string q)
        {
            TempData["query"] = q;
            if (TempData["searchModel"] != null)
                TempData.Keep("searchModel");
            TempData["IsRefresh"] = true;
            return View();
        }
        [HttpGet]
        [Route("search/nquery")]
        public async Task<ActionResult> NIndex(string q)
        {
            TempData["query"] = q;
            if (TempData["searchModel"] != null)
                TempData.Keep("searchModel");
            TempData["IsRefresh"] = true;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SearchResults(string model, string points)
        {
            if (TempData["IsRefresh"] != null)
            {
                if (Convert.ToBoolean(TempData["IsRefresh"]))
                {
                    if (TempData["searchModel"] != null)
                    {
                        string strTempSearchModel = TempData["searchModel"].ToString();
                        UserSearchDTO tmpSearchDto = JsonConvert.DeserializeObject<UserSearchDTO>(strTempSearchModel);
                        UserSearchDTO currSearchDto = JsonConvert.DeserializeObject<UserSearchDTO>(model);
                        if (tmpSearchDto.txtSearchTerm == currSearchDto.txtSearchTerm)
                            model = strTempSearchModel;
                    }
                }
            }

            TempData["searchModel"] = model;
            UserSearchDTO searchDto = JsonConvert.DeserializeObject<UserSearchDTO>(model);
            Points pointsFromClient = JsonConvert.DeserializeObject<Points>(points);
             
            string strLocation = searchDto.hdnlocation;

            IDictionary<string, string> dictFilters = JsonConvert.DeserializeObject<Dictionary<string, string>>(searchDto.hdnSearchFilters);
            SolrSearchQuery solrSearchQuery = new SolrSearchQuery();
            if (pointsFromClient.PaymentType == 1)
            {
                solrSearchQuery.StaticFacets.IsCOD = true;
            }


            solrSearchQuery.Query = searchDto.txtSearchTerm;
            solrSearchQuery.Location = strLocation.Remove(strLocation.LastIndexOf(','));
            solrSearchQuery.Rows = Customer.Utils.SearchUtil.PageSize;
            solrSearchQuery.Start = searchDto.hdnStart;

            if (!string.IsNullOrWhiteSpace(searchDto.hdnSort))
            {
                string[] arSort = searchDto.hdnSort.Split(',');
                solrSearchQuery.SortField = arSort[0];
                solrSearchQuery.SortDirection = arSort[1];
            }
            List<SearchFacetsDTO> lstFacets = new List<SearchFacetsDTO>();

            if (!string.IsNullOrWhiteSpace(searchDto.hdnSearchFilters))
            {
                dynamic d = JsonConvert.DeserializeObject<dynamic>(searchDto.hdnSearchFilters);
                foreach (JProperty uiFacet in d)
                {
                    if (uiFacet.Name.Contains("_"))
                    {
                        if (bool.TryParse(uiFacet.Value.ToString(), out bool blnFacetValue))
                        {
                            if (blnFacetValue)
                            {
                                string strId = uiFacet.Name.Substring(0, uiFacet.Name.IndexOf("_"));
                                SearchFacetsDTO searchFacet = lstFacets.Where(fa => fa.Id.ToString() == strId).FirstOrDefault();

                                string strSearchFilter = uiFacet.Name.Substring(strId.Length + 1);
                                string strSelectedFacet = string.Empty;
                                FacetItem facetItem = null;
                                if (TempData["searchResult"] != null)
                                {
                                    QueryResponse<ProductSearchResult> prevSearchResults = TempData["searchResult"] as QueryResponse<ProductSearchResult>;
                                    FacetsResponse facetsResponse = prevSearchResults.Facets.FirstOrDefault(f => f.Id == Convert.ToInt32(strId));
                                    if (facetsResponse != null)
                                    {
                                        facetItem = facetsResponse.FacetFilters.FirstOrDefault(f => f.FacetTextId.ToString() == strSearchFilter);
                                    }
                                }

                                if (facetItem != null)
                                {
                                    if (searchFacet != null)
                                    {
                                        searchFacet.SelectedFacets.Add(facetItem.FacetText);
                                    }
                                    else
                                    {
                                        lstFacets.Add(new SearchFacetsDTO { Id = Convert.ToInt32(strId), SelectedFacets = new List<string> { facetItem.FacetText } });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bool tmpBool = false;
                        switch (uiFacet.Name)
                        {
                            case "IsCOD":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsCOD = tmpBool;
                                break;
                            case "IsCrPN":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsCCPN = tmpBool;
                                break;
                            case "IsCrPL":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsCCPL = tmpBool;
                                break;
                            case "IsDrPN":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsDCPN = tmpBool;
                                break;
                            case "IsDrPL":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsDCPL = tmpBool;
                                break;
                            case "IsPickup":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsPickup = tmpBool;
                                break;
                            case "IsDelivery":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsDelivery = tmpBool;
                                break;
                            case "IsShipping":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsShipping = tmpBool;
                                break;
                            case "ServiceType":
                                if (uiFacet.Value != null)
                                    if (uiFacet.Value.ToString() != "0")
                                        solrSearchQuery.StaticFacets.ServiceType = Convert.ToInt32(uiFacet.Value);
                                break;
                            case "ItemType":
                                solrSearchQuery.StaticFacets.ItemType = uiFacet.Value.ToString();
                                break;
                            case "Price":
                                if (uiFacet.Value != null)
                                    if (!string.IsNullOrWhiteSpace(uiFacet.Value.ToString()))
                                        solrSearchQuery.StaticFacets.Price = uiFacet.Value.ToString().Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }


            solrSearchQuery.Facets = lstFacets;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    _commonHeader.setHeaders(client);
                }
                catch (Exception ex)
                {

                }
                //CommonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/SearchAPI/SearchStoreByProductAndStoreName", solrSearchQuery);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        responseData = apiRes.SelectToken("Data").ToString();
                        QueryResponse<ProductSearchResult> queryResponse = JsonConvert.DeserializeObject<QueryResponse<ProductSearchResult>>(responseData);
                        if (dictFilters != null)
                        {
                            if (dictFilters["Price"] != null)
                            {
                                string[] arSelectedPrice = dictFilters["Price"].Split(',');
                                if (arSelectedPrice.Length == 2)
                                {
                                    if (Convert.ToDouble(arSelectedPrice[0]) != queryResponse.MinPrice)
                                        queryResponse.MinPrice = Convert.ToDouble(arSelectedPrice[0]);
                                    if (Convert.ToDouble(arSelectedPrice[1]) != queryResponse.MaxPrice)
                                        queryResponse.MaxPrice = Convert.ToDouble(arSelectedPrice[1]);
                                }
                            }
                        }
                        queryResponse.OriginalPriceRange = queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString();

                        bool setSession = false; bool setTotalSearchResults = false;
                        var searchString = _httpContextAccessor.HttpContext.Session.GetString("searchString");
                        //if (Session["searchString"]?.ToString() == queryResponse.OriginalQuery.Query)
                        if (searchString?.ToString() == queryResponse.OriginalQuery.Query)
                        {
                            var PriceRange = _httpContextAccessor.HttpContext.Session.GetString("PriceRange");
                            //if (Session["PriceRange"]?.ToString() != queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString())
                            if (PriceRange?.ToString() != queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString())
                            {
                                string[] arPriceRanges = PriceRange.ToString().Split(',');
                                queryResponse.MinPrice = Convert.ToDouble(arPriceRanges[0]);
                                queryResponse.MaxPrice = Convert.ToDouble(arPriceRanges[1]);
                            }
                            else
                                setSession = true;


                            var TotalSearchResults = _httpContextAccessor.HttpContext.Session.GetString("TotalSearchResults");
                            if (TotalSearchResults != null)
                            {
                                queryResponse.OriginalResultsCount = Convert.ToInt32(TotalSearchResults);
                            }
                            else
                                setTotalSearchResults = true;
                        }
                        else
                        {
                            setSession = setTotalSearchResults = true;
                        }

                        if (setSession)
                        {
                            var searchStringq = _httpContextAccessor.HttpContext.Session.GetString("searchString");
                            var PriceRange = _httpContextAccessor.HttpContext.Session.GetString("PriceRange");
                            var TotalSearchResults = _httpContextAccessor.HttpContext.Session.GetString("TotalSearchResults");
                            //Session["searchString"] = queryResponse.OriginalQuery.Query;
                            //Session["PriceRange"] = queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString();
                            searchStringq = queryResponse.OriginalQuery.Query;
                            PriceRange = queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString();
                            if (setTotalSearchResults)
                            {
                                queryResponse.OriginalResultsCount = queryResponse.NGroups;
                                //Session["TotalSearchResults"] = queryResponse.OriginalResultsCount = queryResponse.NGroups;
                                _httpContextAccessor.HttpContext.Session.SetString("TotalSearchResults", queryResponse.NGroups.ToString());
                            }
                        }

                        //TempData["searchResult"] = queryResponse;
                        var result = await _viewRenderService.RenderToStringAsync("Search/_SearchResultsStoreNest", queryResponse);
                        return Json(new { result = result });
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    AssignValuesToDTO.AssingDToValues(err, ex, "Search/SearchResultsStore", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                }

            }

            return Json(new { result = "down" });
        }
        [HttpPost]
        public async Task<JsonResult> NSearchResults(string model, string points, int showRows, int catId, int subCatId, string sortingOrder="")
        {
            if (TempData["IsRefresh"] != null)
            {
                if (Convert.ToBoolean(TempData["IsRefresh"]))
                {
                    if (TempData["searchModel"] != null)
                    {
                        string strTempSearchModel = TempData["searchModel"].ToString();
                        UserSearchDTO tmpSearchDto = JsonConvert.DeserializeObject<UserSearchDTO>(strTempSearchModel);
                        UserSearchDTO currSearchDto = JsonConvert.DeserializeObject<UserSearchDTO>(model);
                        if (tmpSearchDto.txtSearchTerm == currSearchDto.txtSearchTerm)
                            model = strTempSearchModel;
                    }
                }
            }
            TempData["searchModel"] = model;
            AppUtils.SearchModel = model;

            UserSearchDTO searchDto = JsonConvert.DeserializeObject<UserSearchDTO>(model);
            Points pointsFromClient = JsonConvert.DeserializeObject<Points>(points);

            AppUtils.SearchString = searchDto.txtSearchTerm;

            string strLocation = searchDto.hdnlocation;

            IDictionary<string, string> dictFilters = JsonConvert.DeserializeObject<Dictionary<string, string>>(searchDto.hdnSearchFilters);
            SolrSearchQuery solrSearchQuery = new SolrSearchQuery();
            if (pointsFromClient.PaymentType == 1)
            {
                solrSearchQuery.StaticFacets.IsCOD = true;
            }
            else if (pointsFromClient.PaymentType == 2)
            {
                solrSearchQuery.StaticFacets.IsCCPL = true;
            }
            else if (pointsFromClient.PaymentType == 3)// both which one?
            {
                //solrSearchQuery.StaticFacets.IsCOD = true;
            }


            solrSearchQuery.Query = searchDto.txtSearchTerm;
            solrSearchQuery.Location = strLocation.Remove(strLocation.LastIndexOf(','));
            solrSearchQuery.Rows = showRows;//Customer.Utils.SearchUtil.PageSize;
            solrSearchQuery.Start = searchDto.hdnStart;

            if (!string.IsNullOrWhiteSpace(searchDto.hdnSort))
            {
                string[] arSort = searchDto.hdnSort.Split(',');
                solrSearchQuery.SortField = arSort[0];
                solrSearchQuery.SortDirection = arSort[1];
            }
            List<SearchFacetsDTO> lstFacets = new List<SearchFacetsDTO>();

            if (!string.IsNullOrWhiteSpace(searchDto.hdnSearchFilters))
            {
                dynamic d = JsonConvert.DeserializeObject<dynamic>(searchDto.hdnSearchFilters);
                foreach (JProperty uiFacet in d)
                {
                    if (uiFacet.Name.Contains("_"))
                    {
                        if (bool.TryParse(uiFacet.Value.ToString(), out bool blnFacetValue))
                        {
                            if (blnFacetValue)
                            {
                                string strId = uiFacet.Name.Substring(0, uiFacet.Name.IndexOf("_"));
                                SearchFacetsDTO searchFacet = lstFacets.Where(fa => fa.Id.ToString() == strId).FirstOrDefault();

                                string strSearchFilter = uiFacet.Name.Substring(strId.Length + 1);
                                string strSelectedFacet = string.Empty;
                                FacetItem facetItem = null;
                                if (TempData["searchResult"] != null)
                                {
                                    QueryResponse<ProductSearchResult> prevSearchResults = TempData["searchResult"] as QueryResponse<ProductSearchResult>;
                                    FacetsResponse facetsResponse = prevSearchResults.Facets.FirstOrDefault(f => f.Id == Convert.ToInt32(strId));
                                    if (facetsResponse != null)
                                    {
                                        facetItem = facetsResponse.FacetFilters.FirstOrDefault(f => f.FacetTextId.ToString() == strSearchFilter);
                                    }
                                }

                                if (facetItem != null)
                                {
                                    if (searchFacet != null)
                                    {
                                        searchFacet.SelectedFacets.Add(facetItem.FacetText);
                                    }
                                    else
                                    {
                                        lstFacets.Add(new SearchFacetsDTO { Id = Convert.ToInt32(strId), SelectedFacets = new List<string> { facetItem.FacetText } });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bool tmpBool = false;
                        switch (uiFacet.Name)
                        {
                            case "IsCOD":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsCOD = tmpBool;
                                break;
                            case "IsCrPN":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsCCPN = tmpBool;
                                break;
                            case "IsCrPL":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsCCPL = tmpBool;
                                break;
                            case "IsDrPN":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsDCPN = tmpBool;
                                break;
                            case "IsDrPL":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsDCPL = tmpBool;
                                break;
                            case "IsPickup":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsPickup = tmpBool;
                                break;
                            case "IsDelivery":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsDelivery = tmpBool;
                                break;
                            case "IsShipping":
                                if (bool.TryParse(uiFacet.Value.ToString(), out tmpBool))
                                    solrSearchQuery.StaticFacets.IsShipping = tmpBool;
                                break;
                            case "ServiceType":
                                if (uiFacet.Value != null)
                                    if (uiFacet.Value.ToString() != "0")
                                        solrSearchQuery.StaticFacets.ServiceType = Convert.ToInt32(uiFacet.Value);
                                break;
                            case "ItemType":
                                solrSearchQuery.StaticFacets.ItemType = uiFacet.Value.ToString();
                                break;
                            case "Price":
                                if (uiFacet.Value != null)
                                    if (!string.IsNullOrWhiteSpace(uiFacet.Value.ToString()))
                                        solrSearchQuery.StaticFacets.Price = uiFacet.Value.ToString().Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }


            solrSearchQuery.Facets = lstFacets;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    _commonHeader.setHeaders(client);
                }
                catch (Exception ex)
                {

                }
                //CommonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/SearchAPI/SearchStoreByProductAndStoreName", solrSearchQuery);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        responseData = apiRes.SelectToken("Data").ToString();
                        QueryResponse<ProductSearchResult> queryResponse = JsonConvert.DeserializeObject<QueryResponse<ProductSearchResult>>(responseData);
                        if (dictFilters != null)
                        {
                            if (dictFilters["Price"] != null)
                            {
                                string[] arSelectedPrice = dictFilters["Price"].Split(',');
                                if (arSelectedPrice.Length == 2)
                                {
                                    if (Convert.ToDouble(arSelectedPrice[0]) != queryResponse.MinPrice)
                                        queryResponse.MinPrice = Convert.ToDouble(arSelectedPrice[0]);
                                    if (Convert.ToDouble(arSelectedPrice[1]) != queryResponse.MaxPrice)
                                        queryResponse.MaxPrice = Convert.ToDouble(arSelectedPrice[1]);
                                }
                            }
                        }
                        queryResponse.OriginalPriceRange = queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString();

                        bool setSession = false; bool setTotalSearchResults = false;
                        var searchString = _httpContextAccessor.HttpContext.Session.GetString("searchString");
                        //if (Session["searchString"]?.ToString() == queryResponse.OriginalQuery.Query)
                        if (searchString?.ToString() == queryResponse.OriginalQuery.Query)
                        {
                            var PriceRange = _httpContextAccessor.HttpContext.Session.GetString("PriceRange");
                            //if (Session["PriceRange"]?.ToString() != queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString())
                            if (PriceRange?.ToString() != queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString())
                            {
                                string[] arPriceRanges = PriceRange.ToString().Split(',');
                                queryResponse.MinPrice = Convert.ToDouble(arPriceRanges[0]);
                                queryResponse.MaxPrice = Convert.ToDouble(arPriceRanges[1]);
                            }
                            else
                                setSession = true;


                            var TotalSearchResults = _httpContextAccessor.HttpContext.Session.GetString("TotalSearchResults");
                            if (TotalSearchResults != null)
                            {
                                queryResponse.OriginalResultsCount = Convert.ToInt32(TotalSearchResults);
                            }
                            else
                                setTotalSearchResults = true;
                        }
                        else
                        {
                            setSession = setTotalSearchResults = true;
                        }

                        if (setSession)
                        {
                            var searchStringq = _httpContextAccessor.HttpContext.Session.GetString("searchString");
                            var PriceRange = _httpContextAccessor.HttpContext.Session.GetString("PriceRange");
                            var TotalSearchResults = _httpContextAccessor.HttpContext.Session.GetString("TotalSearchResults");
                            //Session["searchString"] = queryResponse.OriginalQuery.Query;
                            //Session["PriceRange"] = queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString();
                            searchStringq = queryResponse.OriginalQuery.Query;
                            PriceRange = queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString();
                            if (setTotalSearchResults)
                            {
                                queryResponse.OriginalResultsCount = queryResponse.NGroups;
                                //Session["TotalSearchResults"] = queryResponse.OriginalResultsCount = queryResponse.NGroups;
                                _httpContextAccessor.HttpContext.Session.SetString("TotalSearchResults", queryResponse.NGroups.ToString());
                            }
                        }

                        //TempData["searchResult"] = queryResponse;
                        var result = await _viewRenderService.RenderToStringAsync("Search/_SearchResultsStoreNest", queryResponse);
                        return Json(new { result = result });
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    AssignValuesToDTO.AssingDToValues(err, ex, "Search/SearchResultsStore", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                }

            }

            return Json(new { result = "Something is wrong. Please Contact with Administrator." });
        }
    }
}
