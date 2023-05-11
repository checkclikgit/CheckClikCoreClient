using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;
using CheckClikClient.Models;

namespace CheckClikClient.Controllers
{
    public class CouponsController : BaseController
    {

        string currentUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpContextAccessor _hhttpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;
        private readonly IViewRenderService _viewRenderService;

        public CouponsController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService) : base(httpContextAccessor, errorHandler, options, commonHeader)
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

        [Route("Coupons/Index")]
        public ActionResult Index()
        {
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    return View();
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    _errorHandler.WriteError(err, ex.Message);
                    return RedirectToAction("Error");
                }

            }
        }

        [Route("Coupons/IndexNew")]
        public ActionResult IndexN()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CouponsofCustomers(int pageNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    CouponsDTO obj = new CouponsDTO(); 
                    obj.PageNumber = pageNumber;
                    obj.PageSize = int.Parse(_options.PageSize);

                    GetAllCouponsDTO getAllCouponsDTO = new GetAllCouponsDTO(); 
                    getAllCouponsDTO.PageNumber = pageNumber;
                    getAllCouponsDTO.PageSize = int.Parse(_options.PageSize);

                    string api_url = _options.apiurl;
                    string upload_api = _options.UploadLocation;
                    string folder_name = UploadLocations.Stores;

                    //HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/OrdersAPI/NewGetAllCoupons", obj);
                    HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/OrdersAPI/NewGetAllCouponsKM", getAllCouponsDTO);
                    if (responseMessageViewDocuments.IsSuccessStatusCode)
                    {
                        var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        //var totalRecords = response.SelectToken("TotalRecords");
                        var couponsList = JsonConvert.DeserializeObject<List<CouponsDTO>>(Data.ToString());

                        if (couponsList != null)
                        {
                            obj.LstCoupons = couponsList;
                            long toc = couponsList.Count() > 0 ? obj.LstCoupons.FirstOrDefault().TotalRecords : 0;
                            //TempData["TotalCoupons"] = toc;
                            //ViewBag.TotalCoupons = toc;
                            obj.TotalRecords = toc;
                            obj.PageSize = int.Parse(_options.PageSize);
                            decimal d = decimal.Parse(toc.ToString());
                            decimal e = decimal.Parse(obj.PageSize.ToString());
                            decimal f = d / e;
                            string s = f.ToString("0.00", CultureInfo.InvariantCulture);
                            string[] parts = s.Split('.');
                            int i1 = int.Parse(parts[0]);
                            int i2 = int.Parse(parts[1]);
                            if (i2 != 00)
                                obj.pagingNumber = i1 + 1;
                            else
                                obj.pagingNumber = i1;
                        }
                        else
                        {
                            obj.LstCoupons = null;
                            obj.TotalRecords = 0;
                        }
                    }
                    obj.PageNumber = pageNumber;
                    obj.UrlPath = api_url + upload_api + folder_name;

                    var result = await _viewRenderService.RenderToStringAsync("Coupons/CouponsPV", obj);
                    return Json(new { result = result });
                    //return PartialView("CouponsPV", obj);
                }
                catch (Exception ex)
                {
                    return Json(new SelectList("", "Value", "Text"));
                }
            }
        }

    }
}
