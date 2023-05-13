using CheckClikClient.Handlers;
using CheckClikClient.Models;
using Customer;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SearchLibrary.Models;
using SearchLibrary;
using System.Diagnostics;
using CheckClickClient;
using CheckClickClient.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using CheckClikClient;

namespace Customer.Areas.Ar.Controllers
{
    [Area("ar")]
    [Route("ar")]
    public class HomeController : BaseController
    {
        string currentUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;
        private readonly IViewRenderService _viewRenderService;

        public HomeController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService) : base(httpContextAccessor, errorHandler, options, commonHeader)
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

        [Route("Home/Index")]
        public async Task<IActionResult> Index()
        {
            if (AppUtils.CityList.Count() == 0)
            {
                await GetCityDistricts();
            }
            AppUtils.SearchBranchId = "";

            try
            {
                HomeImageDTO obj = new HomeImageDTO();
                obj.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString();
                obj.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).ToString();
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);

                    HttpResponseMessage resMessage = await client.PostAsJsonAsync("api/HomeImageAPI/GetDefaultOrActiveImageList", obj);
                    if (resMessage.IsSuccessStatusCode)
                    {
                        var data = resMessage.Content.ReadAsStringAsync().Result;
                        var json = JObject.Parse(data);
                        var responseData = json.SelectToken("Data").ToString();
                        List<HomeImageDTO> queryResponse = JsonConvert.DeserializeObject<List<HomeImageDTO>>(responseData);
                        if (queryResponse.Count > 1)
                        {
                            obj = queryResponse.Where(x => x.IsItDefault == false).FirstOrDefault();
                        }
                        else
                        {
                            obj = queryResponse.Where(x => x.IsItDefault == true).FirstOrDefault();
                        }
                    }
                    else
                    {
                    }
                }

                return View(obj);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public async Task<IActionResult> NIndex()
        {
            if (AppUtils.CityList.Count() == 0)
            {
                await GetCityDistricts();
            }
            AppUtils.SearchBranchId = "";

            try
            {
                HomeImageDTO obj = new HomeImageDTO();
                obj.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString();
                obj.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).ToString();
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);

                    HttpResponseMessage resMessage = await client.PostAsJsonAsync("api/HomeImageAPI/GetDefaultOrActiveImageList", obj);
                    if (resMessage.IsSuccessStatusCode)
                    {
                        var data = resMessage.Content.ReadAsStringAsync().Result;
                        var json = JObject.Parse(data);
                        var responseData = json.SelectToken("Data").ToString();
                        List<HomeImageDTO> queryResponse = JsonConvert.DeserializeObject<List<HomeImageDTO>>(responseData);
                        if (queryResponse.Count > 1)
                        {
                            obj = queryResponse.Where(x => x.IsItDefault == false).FirstOrDefault();
                        }
                        else
                        {
                            obj = queryResponse.Where(x => x.IsItDefault == true).FirstOrDefault();
                        }
                    }
                    else
                    {
                    }
                }

                return View(obj);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        //public async Task<IActionResult> Index()
        //{
        //    if (AppUtils.CityList.Count() == 0)
        //    {
        //        await GetCityDistricts();
        //    }
        //    AppUtils.SearchBranchId = "";
        //    return View(new HomeImageDTO());
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SearchQ(string q, string txtSearchTerm, string hdnlocation, string hdnSearchText)
        {
            if (!string.IsNullOrWhiteSpace(txtSearchTerm))
                q = txtSearchTerm.Trim();
            if (!string.IsNullOrWhiteSpace(hdnSearchText))
                q = hdnSearchText.Trim();

            if (q == null)
                q = "";
            TempData["location"] = hdnlocation;
            TempData["query"] = q;
            //Response.Redirect("~/search?q=" + q.ToLower());
            //Response.Redirect("~/search/query?q=" + q.ToLower());
            return RedirectToAction("query", "search", new { q = q.ToLower() });
            //QueryResponse<ProductSearchResult> solrResponse = GetSearchResults(q);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NSearchQ(string q, string txtSearchTerm, string hdnlocation, string hdnSearchText)
        {
            if (!string.IsNullOrWhiteSpace(txtSearchTerm))
                q = txtSearchTerm.Trim();
            if (!string.IsNullOrWhiteSpace(hdnSearchText))
                q = hdnSearchText.Trim();

            if (q == null)
                q = "";
            TempData["location"] = hdnlocation;
            TempData["query"] = q;
            //Response.Redirect("~/search?q=" + q.ToLower());
            //Response.Redirect("~/search/query?q=" + q.ToLower());
            return RedirectToAction("Nquery", "search", new { q = q.ToLower() });
            //QueryResponse<ProductSearchResult> solrResponse = GetSearchResults(q);
            return View();
        }
        public async Task<ActionResult> UpdateStoreLocation(string coordinate)
        {
            string[] resp = new string[3];
            try
            {
                //XmlDocument xDoc = new XmlDocument();
                //xDoc.Load("https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + coordinate + "&key=AIzaSyA0-Sj6IFygd93B3rZwoC9VAGQopixGXig");

                //XmlNodeList xNodelst = xDoc.GetElementsByTagName("result");
                //XmlNode xNode = xNodelst.Item(0);
                //string adress = xNode.SelectSingleNode("formatted_address").InnerText;
                //string mahalle = xNode.SelectSingleNode("address_component[3]/long_name").InnerText;
                //string ilce = xNode.SelectSingleNode("address_component[4]/long_name").InnerText;
                //string region = xNode.SelectSingleNode("address_component[5]/long_name").InnerText;

                List<CityDTO> CityList = new List<CityDTO>();
                List<DistrictDTO> DistrictList = new List<DistrictDTO>();
                CustomerDTO obj = new CustomerDTO();
                AppUtils.UserLatLng = coordinate;
                if (!string.IsNullOrEmpty(coordinate))
                {
                    var arr = coordinate.Split(',').ToArray();
                    obj.Latitude = arr[0];
                    obj.Longitude = arr[1];
                }
                //else
                //{       
                //    obj.Latitude = "24.6951";
                //    obj.Latitude = "46.6805";
                //}
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetLocationByLatLng", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var data = JObject.Parse(responseData);
                        if (data != null)
                        {
                            var str = data.SelectToken("Status").ToString();
                            var message = data.SelectToken("Message").ToString();
                            var catdata = data.SelectToken("Data");
                            var SelectedCity = catdata.SelectToken("SelectedCity").ToString();
                            var SelectedDistrict = catdata.SelectToken("SelectedDistrict").ToString();

                            CityList = JsonConvert.DeserializeObject<List<CityDTO>>(SelectedCity);
                            DistrictList = JsonConvert.DeserializeObject<List<DistrictDTO>>(SelectedDistrict);
                            var cityid = CityList.Select(x => x.Id).FirstOrDefault();
                            var districtId = DistrictList.Select(x => x.Id).FirstOrDefault();
                            //resp[0] = region;
                            resp[0] = "";
                            resp[1] = cityid.ToString();
                            resp[2] = districtId.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Json(resp);
        }
        [HttpPost]
        public async Task<JsonResult> Autocomplete(string q, string lang)
        {
            AutocompleteDTO resp = new AutocompleteDTO();
            resp.Status = false;
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/SearchAPI/Autocomplete?query=" + q + "&language=" + lang, q);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var apiRes = JObject.Parse(responseData);
                        responseData = apiRes.SelectToken("Data").ToString();
                        JArray jarSuggestions = JArray.Parse(responseData);
                        List<string> lstSuggestions = jarSuggestions.ToObject<List<string>>();
                        return Json(lstSuggestions);
                    }
                }
                catch (Exception ex)
                {
                    //ErrorLogDTO err = new ErrorLogDTO();
                    //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                    //ErrorHandler.WriteError(err, ex.Message);
                }

            }
            return Json(resp.AutocompleteList);
        }

        public IActionResult TermsAndConditions()
        {
            return View();
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
                        DistrictList = JsonConvert.DeserializeObject<List<DistrictDTO>>(dstlist);
                        slCity = new SelectList(CityList, "Id", "NameEn", null);
                        slDistrict = new SelectList(DistrictList, "Id", "NameEn", null);

                        AppUtils.CityList = CityList;
                        AppUtils.DistrictList = DistrictList;

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


    }
}