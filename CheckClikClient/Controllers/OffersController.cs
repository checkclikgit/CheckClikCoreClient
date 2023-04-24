using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using CheckClickClient;

namespace CheckClikClient.Controllers
{
    public class OffersController : BaseController
    {
        string currentUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpContextAccessor _hhttpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;
        private readonly IViewRenderService _viewRenderService;

        public OffersController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService) : base(httpContextAccessor, errorHandler, options, commonHeader)
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
        //string currentUrl = "";
        //public OffersController()
        //{
        //    var baseURL = System.Configuration.ConfigurationManager.AppSettings["baseUrl"].ToString().ToLower();
        //    currentUrl = System.Web.HttpContext.Current.Request.Url.ToString().ToLower();// Current.Request.Url.AbsolutePath;
        //    currentUrl = currentUrl.Replace(baseURL, baseURL + "ar/");
        //    ViewBag.CurrentURL = currentUrl;
        //}
        // GET: Offers
        [Route("offers/index")]
        public async Task<ActionResult> Index()
        {
            try
            { 
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        public async Task<ActionResult> PVOffer(int CityId = 0, int DistrictId = 0, string Latitude = "", string Longitude = "")
        {
            try
            {
                OffersDTO obj = new OffersDTO();

                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);

                    obj.Latitude = Latitude;
                    obj.Longitude = Longitude;
                    HttpResponseMessage resMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetHomePageDetails", obj);
                    if (resMessage.IsSuccessStatusCode)
                    {
                        var data = resMessage.Content.ReadAsStringAsync().Result;
                        var json = JObject.Parse(data);
                        var catdata = json.SelectToken("Data");

                        var ProductOffers = catdata.SelectToken("Offers").ToString();
                        var ServiceOffers = catdata.SelectToken("Offers2").ToString();
                        var Banners = catdata.SelectToken("HomeBanners").ToString();
                        var stores = catdata.SelectToken("FeaturedStores").ToString();
                        var SubCategory = catdata.SelectToken("SubCategoryList").ToString();

                        obj.listBanners = JsonConvert.DeserializeObject<List<BannersDTO>>(Banners);
                        obj.listStores = JsonConvert.DeserializeObject<List<StoreDTO>>(stores);
                        var ProductOfferList = JsonConvert.DeserializeObject<List<OffersCDTO>>(ProductOffers);
                        var ServiceOfferList = JsonConvert.DeserializeObject<List<OffersCDTO>>(ServiceOffers);
                        //TempData["ProductOfferList"] = ProductOfferList;
                        //TempData["ServiceOfferList"] = ServiceOfferList;
                        AppUtils.ProductOfferList = ProductOfferList;
                        AppUtils.ServiceOfferList = ServiceOfferList;
                        obj.ProductListOffers = ProductOfferList;
                        obj.ServiceListOffers = ServiceOfferList;
                        //TempData["ServiceOfferList"] = ServiceOfferList;
                        List<OffersCDTO> lstoffers = JsonConvert.DeserializeObject<List<OffersCDTO>>(SubCategory).ToList(); 
                        obj.ListOffers = (from item in lstoffers
                                          group item by new
                                          {
                                              item.IconClass,
                                              item.NameEn,
                                              item.Id
                                          } into gcs
                                          select new OffersCDTO()
                                          {
                                              IconClass = gcs.Key.IconClass,
                                              NameEn = gcs.Key.NameEn,
                                              Id = gcs.Key.Id
                                          }).ToList();
                        obj.ApiURL = _options.apiurl;
                    }
                    else
                    {
                        obj.listStores = new List<StoreDTO>();
                        obj.ListOffers = new List<OffersCDTO>();

                        //TempData["ProductOfferList"] = obj.ListOffers;
                        //TempData["ServiceOfferList"] = obj.ListOffers;
                    }
                }

                var result = await _viewRenderService.RenderToStringAsync("Offers/PVOffer", obj);
                return Json(new { result = result });
                //return PartialView(obj);
            }
            catch (Exception ex)
            {
                return PartialView("Error");
            }
        }
        [Route("offers/offerlist")]
        public async Task<ActionResult> OfferList(string Id)
        {
            try
            {
                int BranchId = 0, MainCategoryId = 0, StoreId = 0;
                Id = Id.Replace("-", "/");
                Id = Customer.Utils.StringUtil.URLDecrypt(Id);
                string[] data = new string[4];
                data = Id.Split('_');
                string SubCategoryEn = Convert.ToString(data[0]);
                BranchId = Convert.ToInt32(data[1]);
                MainCategoryId = Convert.ToInt32(data[2]);
                StoreId = Convert.ToInt32(data[3]);

                await PrepareLayout(BranchId);
                var viewmoddel = (BaseViewModel)TempData["Branch"];
                ViewBag.BranchModel = viewmoddel;

                OffersDTO obj = new OffersDTO();
                SessionDTO login = AppUtils.UserLogin;
                if (login == null)
                {
                    SessionDTO ss = new SessionDTO();
                    ss.BranchId = BranchId;
                    ss.StoreId = StoreId;
                    ss.IsLogin = false;
                    ss.UserId = 0;
                    AppUtils.UserLogin = ss;
                }
                else
                {
                    login.BranchId = BranchId;
                    login.StoreId = StoreId;
                    AppUtils.UserLogin = login;
                }
                // List<OffersCDTO> productlistOffer = (List<OffersCDTO>)TempData.Peek("ProductOfferList");
                //List<OffersCDTO> servicelistOffer = (List<OffersCDTO>)TempData.Peek("ServiceOfferList");
                 List<OffersCDTO> productlistOffer = AppUtils.ProductOfferList;
                List<OffersCDTO> servicelistOffer = AppUtils.ServiceOfferList;


                 obj.ProductListOffers = productlistOffer.Where(x => x.CategoryIds.Split(',').Any(y => y.Contains(MainCategoryId.ToString()))).ToList();
                 obj.ServiceListOffers = servicelistOffer.Where(x => x.CategoryIds.Split(',').Any(y => y.Contains(MainCategoryId.ToString()))).ToList();
                obj.SubCategoryEn = SubCategoryEn;
                TempData["SubCategoryId"] = MainCategoryId;
                return View(obj);
            }
            catch (Exception ex)
            {
                Response.Redirect("~/");
                return View();
            }
        }
        [Route("offers/offerlistitem")]
        public async Task<ActionResult> OfferListItem(string Id)
        {
            try
            {
                int BranchId = 0, MainCategoryId = Convert.ToInt32(Id), StoreId = 0; 

                OffersDTO obj = new OffersDTO();
                //List<OffersCDTO> productlistOffer = (List<OffersCDTO>)TempData.Peek("ProductOfferList");
                //List<OffersCDTO> servicelistOffer = (List<OffersCDTO>)TempData.Peek("ServiceOfferList");
                List<OffersCDTO> productlistOffer = AppUtils.ProductOfferList;
                List<OffersCDTO> servicelistOffer = AppUtils.ServiceOfferList;

                obj.ProductListOffers = productlistOffer.Where(x => x.CategoryIds.Split(',').Any(y => y.Contains(MainCategoryId.ToString()))).ToList();
                obj.ServiceListOffers = servicelistOffer.Where(x => x.CategoryIds.Split(',').Any(y => y.Contains(MainCategoryId.ToString()))).ToList();


                var result = await _viewRenderService.RenderToStringAsync("Offers/OfferListItem", obj);
                return Json(new { result = result });

                //return View("OfferListItem", obj);
            }
            catch (Exception ex)
            {
                Response.Redirect("~/");
                return View();
            }
        }

        public ActionResult AllCategories()
        {
            return View();
        }
    }
}
