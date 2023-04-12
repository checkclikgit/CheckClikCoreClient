using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Customer.Utils; 
using CheckClickClient;
using CheckClikClient.Models;
using System.Reflection;
using System.Drawing.Drawing2D;
using Microsoft.CodeAnalysis.Operations;
using CheckClickClient.Models;

namespace CheckClikClient.Controllers
{
    public class BaseController : Controller
    {
        string currentUrl = "";
        string baseUrl = "";
        string Profileurl = "";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader; 

        public BaseController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options.Value;

            var baseURL = _options.baseUrl;
            currentUrl = _httpContextAccessor.HttpContext.Request.PathBase.ToString();
            currentUrl = currentUrl.Replace(baseURL, baseURL + "ar/");
            Profileurl = _options.profileUrl;

            ViewBag.CurrentURL = currentUrl;
            _errorHandler = errorHandler;
            _commonHeader = commonHeader;


            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    CartDTO obj = new CartDTO();
                    //_login = (SessionDTO)System.Web.HttpContext.Current.Session["UserLogin"]; 
                    if (AppUtils.UserLogin != null)
                    {
                        if (AppUtils.UserLogin.IsLogin == true)
                        {
                            List<CartDTO> tolst = new List<CartDTO>();
                            string api_url = _options.apiurl;
                            string upload_api = _options.UploadLocation;
                            string folder_name = UploadLocations.Products;
                            obj.UserId = AppUtils.UserLogin.UserId;
                            //try {
                            //    UserIdDTO p = new UserIdDTO();
                            //    p.UserId = obj.UserId;
                            //    var response1 = Task.Run(async () => await client.PostAsJsonAsync("api/ProductsInfoAPI/NewGetCartItemsTemp", p)).Result;
                            //}
                            //catch (Exception ex)
                            //{ 

                            //}

                            UserIdDTO userIdDto = new UserIdDTO();
                            userIdDto.UserId = obj.UserId;

                            var response = Task.Run(async () => await client.PostAsJsonAsync("api/ProductsInfoAPI/NewGetCartItemsByUserId", userIdDto)).Result;
                            var result = response.Content.ReadAsStringAsync().Result;
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var docs = JsonConvert.DeserializeObject<CartDTO>(result);
                                if (docs.Data != null)
                                {
                                    var jtok = docs.Data;
                                    var jProductListToken = jtok.SelectToken("CartList");

                                    if (jProductListToken.ToString() != "[]")
                                        obj.CartList = JsonConvert.DeserializeObject<List<CartDTO>>(jProductListToken.ToString());
                                    else
                                    {
                                        obj.CartList = new List<CartDTO>();
                                        //_login.CartItems = 0;
                                    }
                                    obj.UrlPath = api_url + upload_api + folder_name;
                                    ViewBag.CartList = obj.CartList;
                                    AppUtils.CartList = obj.CartList;
                                    ViewBag.UrlPath = obj.UrlPath;
                                    AppUtils.UserLogin.CartBranchId = obj.CartList != null && obj.CartList.Count() > 0 ? obj.CartList.FirstOrDefault().BranchId : 0;
                                    //AppUtils.UserLogin.CartItems = 1;
                                    AppUtils.UserLogin.CartItems = 1;
                                }
                                else
                                {
                                    obj.CartList = tolst;
                                    AppUtils.UserLogin.CartBranchId = 0;
                                    AppUtils.UserLogin.CartItems = 0;
                                }
                            }

                            CustomerDTO vnd = new CustomerDTO();
                            vnd.UserId = Convert.ToInt32(AppUtils.UserLogin.UserId);
                            vnd.FlagId = 1;

                            var responseMessage = Task.Run(async () => await client.PostAsJsonAsync("api/CustomerAPI/NewGetNotifications", vnd)).Result;
                            var result1 = responseMessage.Content.ReadAsStringAsync().Result;
                            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                            {

                                var responseData = JObject.Parse(responseMessage.Content.ReadAsStringAsync().Result);
                                bool Status = Convert.ToBoolean(responseData.SelectToken("Status"));
                                var Message = Convert.ToString(responseData.SelectToken("Message"));
                                var Data = responseData.SelectToken("Data");
                                var data = JsonConvert.DeserializeObject<List<NotificationDTO>>(Data.ToString());
                                ViewData["Nofications"] = data;
                            }
                            else
                            {
                                var data = JsonConvert.DeserializeObject<List<NotificationDTO>>("[]");
                                ViewData["Nofications"] = data;
                            }
                        }
                        else
                        {
                            ViewBag.CartList = obj.CartList;
                            ViewBag.UrlPath = obj.UrlPath;
                            var data = JsonConvert.DeserializeObject<List<NotificationDTO>>("[]");
                            ViewData["Nofications"] = data;
                        }
                    }
                    else
                    {
                        ViewBag.CartList = obj.CartList;
                        ViewBag.UrlPath = obj.UrlPath;
                        var data = JsonConvert.DeserializeObject<List<NotificationDTO>>("[]");
                        ViewData["Nofications"] = data;
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            //if (AppUtils.CityList.Count() == 0)
            //{
            //    GetCityDistricts();
            //}

        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public  HttpContext Current => _httpContextAccessor.HttpContext;

        public async Task<string> GetCityByCountryId()
        {
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    StoreDTO obj = new StoreDTO();
                    List<CityDTO> CityList = new List<CityDTO>();
                    var list = new List<SelectListItem>();

                    SelectList slCity = new SelectList("", "Id", "NameEn", null);
                    HttpResponseMessage vendorsResponseMessage = await client.PostAsJsonAsync("api/CityAPI/NewGetCityDetails", obj);
                    if (vendorsResponseMessage.IsSuccessStatusCode)
                    {
                        var responseData = vendorsResponseMessage.Content.ReadAsStringAsync().Result;
                        CityList = JsonConvert.DeserializeObject<List<CityDTO>>(responseData);

                        var json = JsonConvert.SerializeObject(CityList);
                        return json;
                    }
                    else
                    { 
                        return "";
                    }

                }
                catch (Exception ex)
                {
                    SelectList slCity = new SelectList("", "Id", "NameEn", null); 
                    ViewBag.ListCity = JsonConvert.SerializeObject(new CityDTO());
                    return "";

                }
            }
        }

        public async Task<string> GetCityDistricts()
        {
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    StoreDTO obj = new StoreDTO();
                    List<CityDTO> CityList = new List<CityDTO>();
                    List<DistrictDTO> DistrictList = new List<DistrictDTO>();
                      
                    SelectList slCity = new SelectList("", "Id", "NameEn", null);
                    SelectList slDistrict = new SelectList("", "Id", "NameEn", null);
                    HttpResponseMessage vendorsResponseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetCountryDistricts", obj);
                    if (vendorsResponseMessage.IsSuccessStatusCode)
                    {
                        var responseData = vendorsResponseMessage.Content.ReadAsStringAsync().Result;

                        JObject jobj = JObject.Parse(responseData);
                        var data = jobj.SelectToken("Data");
                        var ctlist = data.SelectToken("CityList").ToString();
                        var dstlist = data.SelectToken("DistrictList").ToString();

                        CityList = JsonConvert.DeserializeObject<List<CityDTO>>(ctlist);
                        //DistrictList = JsonConvert.DeserializeObject<List<DistrictDTO>>(dstlist);
                        slCity = new SelectList(CityList, "Id", "NameEn", null);
                        slDistrict = new SelectList(DistrictList, "Id", "NameEn", null);
                        try {
                            AppUtils.UserLogin = new SessionDTO();
                            AppUtils.CityList = CityList.ToList();
                        }
                        catch (Exception ex)
                        { 
                        
                        }
                        
                        //AppUtils.DistrictList = DistrictList;
                        //Session["DefaultCityList"] = slCity;
                        //Session["DefaultDistrictList"] = slDistrict;
                        //Session["DefaultCityList_New"] = CityList;

                        var json = JsonConvert.SerializeObject(CityList);

                        string city = AppUtils.SelectedCity; //(String)Session["SelectedCity"];
                        string district = AppUtils.SelectedDistrict;// (String)Session["SelectedDistrict"];

                        if (city == null && district == null)
                        {
                            //Session["SelectedCity"] = "0";
                            //Session["SelectedDistrict"] = "0";
                            AppUtils.SelectedCity = "0";
                            AppUtils.SelectedDistrict = "0";
                        }

                        return json;

                    }
                    else
                    { 
                        return "";
                    }


                }
                catch (Exception ex)
                {
                    SelectList slCity = new SelectList("", "Id", "NameEn", null);
                    //ViewBag.ListCity = slCity;
                    ViewBag.ListCity = JsonConvert.SerializeObject(new CityDTO());
                    return "";

                }
            }
        }
        //public async Task PrepareLayout(object branchid, string branchName = "", string Latitude = "", string Longitude = "")
        //{
        //    BaseViewModel obj = new BaseViewModel();
        //    using (HttpClient client = new HttpClient())
        //    {
        //        CustomerDTO objs = new CustomerDTO();
        //        objs.BranchId = (int)branchid;
        //        objs.StoreName = branchName;
        //        objs.Latitude = Latitude;
        //        objs.Longitude = Longitude;
        //        SessionDTO login = AppUtils.UserLogin;
        //        //SessionDTO login = (SessionDTO)Session["UserLogin"];
        //        if (login != null && login.IsLogin == true)
        //        {
        //            objs.UserId = login.UserId;
        //        }
        //        _commonHeader.setHeaders(client);
        //        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetBaseBranchDetails", objs);
        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
        //            var json = JObject.Parse(responseData);
        //            var catdata = json.SelectToken("Data");
        //            var storetoken = catdata.SelectToken("Branch").ToString();

        //            var lstStores = JsonConvert.DeserializeObject<List<BaseViewModel>>(storetoken);
                     
        //            if (lstStores != null)
        //            {
        //                if (lstStores.Count != 0)
        //                {
        //                    obj = lstStores[0];
        //                    obj.BranchShift = JsonConvert.DeserializeObject<List<ShiftTimingDTO>>(obj.strBranchShift);
        //                    //Session["SearchBranchId"] = obj.Id;
        //                    AppUtils.SearchBranchId = obj.Id.ToString();
        //                }
        //                else
        //                    obj = new BaseViewModel();
        //            }
        //        }
        //        AppUtils.StoreChatingList = obj.ChatData;
        //        TempData["Branch"] = obj;

        //        if (!(AppUtils.SupportGroupId != null))
        //        {
        //            AppUtils.SupportGroupId = "";
        //        }
        //        else
        //        {
        //            String suppGrp = AppUtils.SupportGroupId.ToString();
        //        }
        //    }
        //}

        public string ChangeCityDistrict(int CityId, int DistrictId)
        {
            //Session["SelectedCity"] = CityId;
            //Session["SelectedDistrict"] = DistrictId; 
            AppUtils.SelectedCity = CityId.ToString();
            AppUtils.SelectedDistrict = DistrictId.ToString(); 
            return "";
        }
        [HttpPost]
        public async Task<ActionResult> AddDeleteFavorite(int Type, int StatusId, string ItemId, int SubCategoryId = 0)
        {
            var status = "false";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    //MyOrdersDTO obj = new MyOrdersDTO();
                    FavoriteDTO obj = new FavoriteDTO();
                    SessionDTO login = AppUtils.UserLogin;
                    //SessionDTO login = (SessionDTO)Session["UserLogin"];
                    if (login != null && login.UserId != 0)
                    { 

                        obj.Type = Type;
                        obj.StatusId = StatusId;
                        obj.Ids = ItemId;
                        obj.SubCategoryId = SubCategoryId;
                        obj.UserId = login.UserId;
                        obj.BranchId = login.BranchId;

                    }
                    else
                    {
                        return Json(new { success = status });
                    }

                    //HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/OrdersAPI/NewAddGetDeleteUserFavorite", obj);
                    //kamrul
                    HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/OrdersAPI/NewAddGetDeleteUserFavoriteNew", obj);
                    if (responseMessageViewDocuments.IsSuccessStatusCode)
                    {
                        var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                        var docs = JsonConvert.DeserializeObject<MyOrdersDTO>(responseData);
                        var jobj = JObject.Parse(responseData);
                        status = jobj.SelectToken("Status").ToString();
                        status = "true";
                    }
                    return Json(new { success = status });
                }
                catch (Exception ex)
                {
                    return Json(new { success = status });
                }
            }
        }

        [Route("{%}/aboutus/{Id}")]
        public async Task<ActionResult> AboutUs(string Id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);

                    Id = Id.Replace(" ", "+");
                    Id = Id.Replace("-", "/");
                    Id = Customer.Utils.StringUtil.URLDecrypt(Id); 

                    await PrepareLayout(Convert.ToInt32(Id), "");
                    var viewmoddel = (BaseViewModel)TempData["Branch"];
                    ViewBag.BranchModel = viewmoddel;
                    List<BaseViewModel> lst = new List<BaseViewModel>();
                    if (!string.IsNullOrEmpty(viewmoddel.OtherBranches))
                    {
                        lst = JsonConvert.DeserializeObject<List<BaseViewModel>>(viewmoddel.OtherBranches);
                    }
                    //lst.Add(viewmoddel);

                    return View();

                } 
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "home");
            }
        }

        [Route("{%}/terms/{Id}")]
        public async Task<ActionResult> Terms(string Id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    Id = Id.Replace(" ", "+");
                    Id = Id.Replace("-", "/");
                    Id = Customer.Utils.StringUtil.URLDecrypt(Id); 

                    await PrepareLayout(Convert.ToInt32(Id), "");
                    var viewmoddel = (BaseViewModel)TempData["Branch"];
                    ViewBag.BranchModel = viewmoddel;
                    List<BaseViewModel> lst = new List<BaseViewModel>();
                    if (!string.IsNullOrEmpty(viewmoddel.OtherBranches))
                    {
                        lst = JsonConvert.DeserializeObject<List<BaseViewModel>>(viewmoddel.OtherBranches);
                    }
                    //lst.Add(viewmoddel);

                    return View();

                } 
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "home");
            }
        }

        [Route("{%}/desc/{Id}")]
        public async Task<ActionResult> Desc(string Id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    Id = Id.Replace(" ", "+");
                    Id = Id.Replace("-", "/");
                    Id = Customer.Utils.StringUtil.URLDecrypt(Id); 

                    await PrepareLayout(Convert.ToInt32(Id), "");
                    var viewmoddel = (BaseViewModel)TempData["Branch"];
                    ViewBag.BranchModel = viewmoddel;
                    List<BaseViewModel> lst = new List<BaseViewModel>();
                    if (!string.IsNullOrEmpty(viewmoddel.OtherBranches))
                    {
                        lst = JsonConvert.DeserializeObject<List<BaseViewModel>>(viewmoddel.OtherBranches);
                    } 

                    return View();

                } 
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "home");
            }
        }

        [Route("{%}/return-policy/{Id}")]
        public async Task<ActionResult> ReturnPolicy(string Id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    Id = Id.Replace(" ", "+");
                    Id = Id.Replace("-", "/");
                    Id = Customer.Utils.StringUtil.URLDecrypt(Id);

                    await PrepareLayout(Convert.ToInt32(Id), "");
                    var viewmoddel = (BaseViewModel)TempData["Branch"];

                    try
                    {
                        if (viewmoddel.TermsAndConditionsEn != null && viewmoddel.TermsAndConditionsEn != "")
                        {
                            viewmoddel.NoFaultStoreArr = JsonConvert.DeserializeObject<List<BranchDTO>>(JArray.Parse(viewmoddel.TermsAndConditionsEn).ToString()).Select(x => x.NoFaultStore).ToArray();
                        }
                        if (viewmoddel.TermsAndConditionsAr != null && viewmoddel.TermsAndConditionsAr != "")
                        {
                            viewmoddel.NonReturnableProductArr = JsonConvert.DeserializeObject<List<BranchDTO>>(JArray.Parse(viewmoddel.TermsAndConditionsAr).ToString()).Select(x => x.NonReturnableProduct).ToArray();
                        }
                        if (viewmoddel.ReturnPolicyAr != null && viewmoddel.ReturnPolicyAr != "")
                        {
                            viewmoddel.ReturnPolicyArArr = JsonConvert.DeserializeObject<List<BranchDTO>>(JArray.Parse(viewmoddel.ReturnPolicyAr).ToString()).Select(x => x.ReturnConditions).ToArray();
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    ViewBag.BranchModel = viewmoddel;
                    //List<BaseViewModel> lst = new List<BaseViewModel>();
                    //if (!string.IsNullOrEmpty(viewmoddel.OtherBranches))
                    //{
                    //    lst = JsonConvert.DeserializeObject<List<BaseViewModel>>(viewmoddel.OtherBranches);
                    //}
                    ////lst.Add(viewmoddel);

                    return View();

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "home");
            }
        }

        //[Route("{%}/ratings")]
        public async Task<PartialViewResult> PVStoreReview(int PageNumber)
        {
            ReviewDTO obj = new ReviewDTO();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    int BranchId = 0;
                    //var sd = (SessionDTO)Session["UserLogin"];
                    var sd = AppUtils.UserLogin;
                    if (sd != null)
                        BranchId = sd.BranchId;
                    _commonHeader.setHeaders(client);
                    obj.Type = 1;
                    obj.StatusId = 3;
                    obj.BranchId = BranchId;
                    obj.ApiUrl = _options.apiurl;
                    obj.PageSize = int.Parse(_options.PageSize);
                    obj.PageNumber = PageNumber;
                    obj.list = new List<ReviewDTO>();
                    HttpResponseMessage responseMessageReviewsList = await client.PostAsJsonAsync("api/OrdersAPI/NewAddGetUserReviews", obj);
                    if (responseMessageReviewsList.IsSuccessStatusCode)
                    {
                        var responseData = responseMessageReviewsList.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        var reviewsList = JsonConvert.DeserializeObject<List<ReviewDTO>>(Data.ToString());
                        if (reviewsList != null && reviewsList.Count() > 0)
                        {

                            obj.list = reviewsList;
                            int toc = reviewsList != null && reviewsList.Count() > 0 ? reviewsList.Count : 0;
                            int count = toc / Convert.ToInt32(obj.PageSize);
                            obj.pagingNumber = Convert.ToDecimal(Convert.ToDecimal(toc) / obj.PageSize) > Convert.ToDecimal(Convert.ToInt32(Convert.ToDecimal(toc) / obj.PageSize)) ? Convert.ToInt32((toc / obj.PageSize)) + 1 : Convert.ToInt32((toc / obj.PageSize));
                            //ViewData["Reviews"] = reviewsList;
                            return PartialView(obj);
                        }
                        else
                        {
                            //   ViewData["Reviews"] = null;
                            return PartialView(obj);
                        }
                    }
                    return PartialView(obj);

                }
            }
            catch (Exception ex)
            {
                return PartialView(obj);
            }
        }
        public async Task<JsonResult> IsLogin()
        {
            bool resp = false;
            if (AppUtils.UserLogin == null)
            {
                resp = false;
            }
            else if (AppUtils.UserLogin != null)
            {
                var userLogin = AppUtils.UserLogin; //(Customer.Models.SessionDTO)Session["UserLogin"];
                if (userLogin.IsLogin == true)
                {
                    resp = true;
                }
                else
                {
                    resp = false;
                }
            }

            return Json(resp);
        }

        public async Task<ActionResult> MapReload(string map)
        {
            if (!string.IsNullOrEmpty(map))
                ViewBag.MapData = JsonConvert.SerializeObject(JArray.Parse(map));
            else
                ViewBag.MapData = map;
            return PartialView();
        }

        public async Task PrepareLayout(object branchid, string branchName = "", string Latitude = "", string Longitude = "")
        {

            BranchDTO branchDTO = new BranchDTO();
            BaseViewModel obj = new BaseViewModel();
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
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetBranchMainSubCatgoryCommon", objs);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var json = JObject.Parse(responseData);
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
                            try
                            {
                                _httpContextAccessor.HttpContext.Session.SetString(SessionKeyConstants.SEARCH_BRAND_ID, obj.Id.ToString());
                                //AppUtils.SearchBranchId = obj.Id.ToString();
                            }
                            catch (Exception ex)
                            {

                            }
                            AppUtils.SearchBranchId = obj.Id.ToString();
                        }
                        else
                            obj = new BaseViewModel();
                    }

                    var lstMainCategories = JsonConvert.DeserializeObject<List<MainCategoryDTO>>(cattoken);
                    var lstSubCategories = JsonConvert.DeserializeObject<List<SubCategoryDTO>>(subCattoken);
                    //try {
                    //    var lstBanners1 = JsonConvert.DeserializeObject<List<BannersDTO>>(Advtoken);
                    //}
                    //catch (Exception ex)
                    //{ 
                    
                    //}
                    var lstBanners = JsonConvert.DeserializeObject<List<BannersDTO>>(Advtoken);

                    branchDTO.listCategory = lstMainCategories;
                    branchDTO.listSubCategory = lstSubCategories;
                    branchDTO.listBanner = lstBanners;
                    //branchDTO.listCategory.Select(S => { S.BranchId = BranchId; return S; }).ToList();

                }
                AppUtils.StoreChatingList = obj.ChatData;
                //TempData["Branch"] = obj;
                AppUtils.Branch = obj;

                if (!(AppUtils.SupportGroupId != null))
                {
                    AppUtils.SupportGroupId = "";
                }
                else
                {
                    String suppGrp = AppUtils.SupportGroupId.ToString();
                }
            }
        }

        public async  Task<List<CountryDTO>> getListOfCountry(CountryDTO obj)
        {
            List<CountryDTO> CountryList = new List<CountryDTO>();
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    // obj.Id = 0;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CountryAPI/NewGetCountryDetails", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var Data = JsonConvert.DeserializeObject<List<CountryDTO>>(responseData);
                        CountryList = Data;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return CountryList;
        }

        public async Task<List<CitysDTO>> getListOfCity(CitysDTO obj)
        {
            List<CitysDTO> CitysList = new List<CitysDTO>();
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    // obj.Id = 0;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CityAPI/NewGetCityDetails", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var Data = JsonConvert.DeserializeObject<List<CitysDTO>>(responseData);
                        CitysList = Data;

                    }
                }
                catch (Exception ex)
                {
                }
            }
            return CitysList;
        }
    }
}
