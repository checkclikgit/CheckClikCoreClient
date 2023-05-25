using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SearchLibrary.Models;
using SearchLibrary;
using CheckClickClient.Models;
using CheckClikClient.Models;
using System.Collections.Generic;
using CheckClickClient;
using CheckClikClient;

namespace Customer.Areas.Ar.Controllers
{
    [Area("ar")]
    [Route("ar")]
    public class ProductController : BaseController
    {

        string currentUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpContextAccessor _hhttpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;
        private readonly IViewRenderService _viewRenderService;

        public ProductController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService): base(httpContextAccessor, errorHandler, options, commonHeader)
        {
            _httpContextAccessor = httpContextAccessor;
            _hhttpContextAccessor = httpContextAccessor;
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
        [Route("Product/List")]
        public async Task<ActionResult> Index(int storeId, string q)
        {
            TempData["query"] = q;
            if (TempData["searchModel"] != null)
                TempData.Keep("searchModel");
            TempData["IsRefresh"] = true;

            List<BranchDTO> lstBranchDTO = new List<BranchDTO>(); 
            lstBranchDTO = await GetBranchListByStoreIdAsync(storeId);
             
            return View(lstBranchDTO);
        } 
        [HttpGet]
        [Route("Product/ListNest")]
        public async Task<ActionResult> IndexNest(int storeId, string q)
        {
            TempData["query"] = q;
            if (TempData["searchModel"] != null)
                TempData.Keep("searchModel");
            TempData["IsRefresh"] = true;

            List<BranchDTO> lstBranchDTO = new List<BranchDTO>(); 
            lstBranchDTO = await GetBranchListByStoreIdAsync(storeId);
             
            return View(lstBranchDTO);
        }
        [Route("Product/CategoryDetails")]
        public async Task<ActionResult> CategoryDetails(int storeId, string q)
        {
            return View();
        }
        public async Task<ActionResult> StoreContactUs(int storeId, string q)
        {
            return View();
        }
        public async Task<ActionResult> NewCheckOutPage(int storeId, string q)
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> GetCatSubCatTiming(int branchid, string search="", string branchName = "", string Latitude = "", string Longitude = "")
        {
            BranchDTO branchDTO = new BranchDTO();
            BaseViewModel obj = new BaseViewModel();
            //var json = PrepareLayoutAsync(Convert.ToInt32("2")).Result;
            var json = PrepareLayoutAsync(branchid).Result;

            var data = json.SelectToken("Data");
            var branchTimingtoken = data.SelectToken("BranchTiming").ToString();
            var cattoken = data.SelectToken("MainCategory").ToString();
            var subCattoken = data.SelectToken("SubCategory").ToString();
            var Advtoken = data.SelectToken("Advertisements").ToString();

            var lstStores = JsonConvert.DeserializeObject<List<BaseViewModel>>(branchTimingtoken);

            if (lstStores != null)
            {
                if (lstStores.Count != 0)
                {
                    obj = lstStores[0];
                    obj.BranchShift = JsonConvert.DeserializeObject<List<ShiftTimingDTO>>(obj.strBranchShift);

                    AppUtils.SearchBranchId = obj.Id.ToString();
                }
                else
                    obj = new BaseViewModel();
            }

            var lstMainCategories = JsonConvert.DeserializeObject<List<MainCategoryDTO>>(cattoken);
            var lstSubCategories = JsonConvert.DeserializeObject<List<SubCategoryDTO>>(subCattoken);
            var lstBanners = JsonConvert.DeserializeObject<List<BannersDTO>>(Advtoken);

            branchDTO.listCategory = lstMainCategories;
            branchDTO.listSubCategory = lstSubCategories;
            branchDTO.listBanner = lstBanners;
            //branchDTO.listCategory.Select(S => { S.BranchId = BranchId; return S; }).ToList();


            AppUtils.StoreChatingList = obj.ChatData;
            //TempData["Branch"] = obj;
            AppUtils.Branch = obj;

            //if (!(AppUtils.SupportGroupId != null))
            //{
            //    AppUtils.SupportGroupId = "";
            //}
            //else
            //{
            //    String suppGrp = AppUtils.SupportGroupId.ToString();
            //}



            var resultCatAndSubCat = await _viewRenderService.RenderToStringAsync("Product/PVSideCategoryAndSubCategory", branchDTO);
            //var resultCatAndSubCat = await _viewRenderService.RenderToStringAsync("Search/_SearchResults", new QueryResponse<ProductSearchResult>());
            var resultTiming = await _viewRenderService.RenderToStringAsync("Product/PVBranchTimings", obj);
            return Json(new { resultCatAndSubCat = resultCatAndSubCat, resultTiming = resultTiming }); 
        }

        [HttpPost]
        public async Task<ActionResult> ProductSearchResults(string model, string branchid, string ProductSearchText, int showRows, string catId, string subCatId, bool clearSession)
        {
            if (clearSession)
            {
                AppUtils.SearchModel = "";
            }
            if (TempData["IsRefresh"] != null)
            {
                if (Convert.ToBoolean(TempData["IsRefresh"]))
                {
                    if (TempData["searchModel"] != null)
                    {
                        string strTempSearchModel = TempData["searchModel"].ToString();
                        UserSearchDTO tmpSearchDto = JsonConvert.DeserializeObject<UserSearchDTO>(strTempSearchModel);
                        //try {
                        //    UserSearchDTO currSearchDt1o = JsonConvert.DeserializeObject<UserSearchDTO>(model);
                        //}
                        //catch (Exception ex) 
                        //{ 
                        
                        //}
                        UserSearchDTO currSearchDto = JsonConvert.DeserializeObject<UserSearchDTO>(model);
                        //if (tmpSearchDto.txtSearchTerm == currSearchDto.hdnSearchText)
                        //    model = strTempSearchModel;
                    }
                }
            }
            if (!string.IsNullOrEmpty(model) && model != "{}")
            {
                TempData["searchModel"] = model;
                AppUtils.SearchModel = model;
            }
            else {
                model = AppUtils.SearchModel.ToString();
            }

            UserSearchDTO searchDto = JsonConvert.DeserializeObject<UserSearchDTO>(model);
            //searchDto.txtSearchTerm = ProductSearchText;
             

            string strLocation = searchDto.hdnlocation;

            IDictionary<string, string> dictFilters = JsonConvert.DeserializeObject<Dictionary<string, string>>(searchDto.hdnSearchFilters);
            SolrSearchQuery solrSearchQuery = new SolrSearchQuery();
            solrSearchQuery.Query = searchDto.hdnSearchText;
            solrSearchQuery.Location = strLocation.Remove(strLocation.LastIndexOf(','));
            //solrSearchQuery.Rows = Customer.Utils.SearchUtil.PageSize;
            solrSearchQuery.Rows = showRows == 0 ? Customer.Utils.SearchUtil.PageSize : showRows;
            solrSearchQuery.Start = searchDto.hdnStart;

            if (!string.IsNullOrEmpty(branchid))
            {
                solrSearchQuery.BranchId = branchid;
            }
            if (!string.IsNullOrEmpty(catId))
            {
                solrSearchQuery.CatId = catId;
            }
            if (!string.IsNullOrEmpty(subCatId))
            {
                solrSearchQuery.SubCatId = subCatId;
            }

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
                                if (AppUtils.SearchResult != null)
                                {
                                    QueryResponse<ProductSearchResult> prevSearchResults = AppUtils.SearchResult;//TempData["searchResult"] as QueryResponse<ProductSearchResult>;
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
                _commonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/SearchAPI/Search", solrSearchQuery);
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
                        if (AppUtils.SearchString == queryResponse.OriginalQuery.Query)
                        {
                            if (AppUtils.PriceRange != queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString())
                            {
                                string[] arPriceRanges = AppUtils.PriceRange.Split(',');
                                queryResponse.MinPrice = Convert.ToDouble(arPriceRanges[0]);
                                queryResponse.MaxPrice = Convert.ToDouble(arPriceRanges[1]);
                            }
                            else
                                setSession = true;

                            if (AppUtils.TotalSearchResults != null)
                            {
                                queryResponse.OriginalResultsCount = Convert.ToInt32(AppUtils.TotalSearchResults);
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
                            AppUtils.SearchString = queryResponse.OriginalQuery.Query;
                            AppUtils.PriceRange = queryResponse.MinPrice.ToString() + "," + queryResponse.MaxPrice.ToString();
                            if (setTotalSearchResults)
                                AppUtils.TotalSearchResults = queryResponse.OriginalResultsCount = queryResponse.NGroups;
                        }

                        //TempData["searchResult"] = queryResponse;
                        AppUtils.SearchResult = queryResponse;
                        var result = await _viewRenderService.RenderToStringAsync("Product/_SearchResults", queryResponse);
                        return Json(new { result = result.ToString() });
                        //////return PartialView("_SearchResults", queryResponse);
                        ////var result = await _viewRenderService.RenderToStringAsync("Search/_SearchResults", new QueryResponse<ProductSearchResult>());
                        ////return Json(new { result = result });
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    AssignValuesToDTO.AssingDToValues(err, ex, "Search/SearchResults", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                }

            }
            var result1 = await _viewRenderService.RenderToStringAsync("Search/_SearchResults", new QueryResponse<ProductSearchResult>());
            return Json(new { result = result1 });
        }


        public async Task<JObject> PrepareLayoutAsync(object branchid, string branchName = "", string Latitude = "", string Longitude = "")
        {

            //BranchDTO branchDTO = new BranchDTO();
            //BaseViewModel obj = new BaseViewModel();
            using (HttpClient client = new HttpClient())
            {
                CustomerDTO objs = new CustomerDTO();
                objs.BranchId = (int)branchid;
                objs.StoreName = branchName;
                objs.Latitude = Latitude;
                objs.Longitude = Longitude;
                SessionDTO login = AppUtils.UserLogin;
                if (login != null && login.IsLogin == true)
                {
                    objs.UserId = login.UserId;
                }
                _commonHeader.setHeaders(client);
                //HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetBaseBranchDetails", objs);
                //var temp = _httpContextAccessor.HttpContext;
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetBranchMainSubCatgoryCommon", objs);
                if (responseMessage.IsSuccessStatusCode)
                { 
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var json = JObject.Parse(responseData);
                    return json;
                    
                }
                return new JObject();
                ////AppUtils.StoreChatingList = obj.ChatData;
                //////TempData["Branch"] = obj;
                ////AppUtils.Branch = obj;

                ////if (!(AppUtils.SupportGroupId != null))
                ////{
                ////    AppUtils.SupportGroupId = "";
                ////}
                ////else
                ////{
                ////    String suppGrp = AppUtils.SupportGroupId.ToString();
                ////}
            }
        }
        //[HttpPost]
        //public async 

        private async Task<List<BranchDTO>> GetBranchListByStoreIdAsync(int storeId)
        {
            using (HttpClient client = new HttpClient())
            {
                List<BranchDTO> lstStoreDTOS = new List<BranchDTO>();
                _commonHeader.setHeaders(client);
                try
                {
                    BranchListSearchDTO branchobj = new BranchListSearchDTO();
                    branchobj.StoreId = storeId;

                    UserRegistrationDTO userRegistrationDTO = new UserRegistrationDTO();
                    userRegistrationDTO.LoginName = "avc";
                    userRegistrationDTO.Password = "abc";

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/BranchAPI/GetBranchListByStoreId", branchobj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        var branchToken = Data.SelectToken("Branches");

                        lstStoreDTOS = JsonConvert.DeserializeObject<List<BranchDTO>>(branchToken.ToString());

                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    AssignValuesToDTO.AssingDToValues(err, ex, "Search/SearchResultsStore", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                }
                return lstStoreDTOS;
            }
        }
        private async Task<CategorySubCategory> GetCategoryAndSubCategoryListByBranchId(int storeId, int BranchId)
        {
            using (HttpClient client = new HttpClient())
            {
                CategorySubCategory lstCategoryAndSubCategory = new CategorySubCategory();
                _commonHeader.setHeaders(client);
                try
                {
                    BranchListSearchDTO branchobj = new BranchListSearchDTO();
                    branchobj.StoreId = storeId;
                    branchobj.BranchId = BranchId;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/BranchAPI/GetCategoryAndSubCategoryListByBranchId", branchobj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        var MapCategoryData = Data.SelectToken("Category").ToString();
                        var MapSubCategoryData = Data.SelectToken("Subcategory").ToString();
                        List<Category> lstCategory = JsonConvert.DeserializeObject<List<Category>>(MapCategoryData.ToString());
                        List<SubCategory> lstSubCategory = JsonConvert.DeserializeObject<List<SubCategory>>(MapSubCategoryData.ToString());
                        lstCategoryAndSubCategory.lstCategories = lstCategory;
                        lstCategoryAndSubCategory.lstSubCategories = lstSubCategory;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    AssignValuesToDTO.AssingDToValues(err, ex, "Search/SearchResultsStore", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                }
                return lstCategoryAndSubCategory;
            }
        }

        [HttpPost]
        public async Task<JsonResult> BranchListByStoreId(string model)
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
            string strLocation = searchDto.hdnlocation;

            IDictionary<string, string> dictFilters = JsonConvert.DeserializeObject<Dictionary<string, string>>(searchDto.hdnSearchFilters);
            SolrSearchQuery solrSearchQuery = new SolrSearchQuery();
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
                        var result = await _viewRenderService.RenderToStringAsync("Search/_SearchResultsStore", queryResponse);
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
    }
}

