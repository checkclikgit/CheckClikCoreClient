using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using CheckClickClient;

namespace CheckClikClient.Controllers
{
    public class WishListController :  BaseController
    {
        string currentUrl = "";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpContextAccessor _hhttpContextAccessor;
    private ErrorHandler _errorHandler;
    private readonly AppSettingsKeysInfo _options;
    private CommonHeader _commonHeader;
    private readonly IViewRenderService _viewRenderService;

    public WishListController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService) : base(httpContextAccessor, errorHandler, options, commonHeader)
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
    // GET: WishList
    [Route("wishlist/index")]
    public async Task<ActionResult> Index()
    {
        using (HttpClient client = new HttpClient())
        {

            bool status = false;
            _commonHeader.setHeaders(client);
            try
            {

                FavoritesDTO obj = new FavoritesDTO();
                SessionDTO login = AppUtils.UserLogin;
                if (login == null)
                {
                    return RedirectToAction("IndexNest", "User"); // later call login form
                }
                else
                {
                    obj.UserId = login.UserId; 
                }
                obj.Type = 1;

                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewAddGetDeleteUserFavorite", obj);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var response = JObject.Parse(responseData);
                    var Data = response.SelectToken("Data");
                    var Message = Convert.ToString(response.SelectToken("Message"));
                    status = Convert.ToBoolean(response.SelectToken("Status").ToString());
                    var storeDetails = Data.SelectToken("StoreList").ToString();
                    var productDetails = Data.SelectToken("ProductList").ToString();
                    var serviceDetails = Data.SelectToken("ServiceList").ToString();
                    var lstStoreDetails = JsonConvert.DeserializeObject<List<FavoritesDTO>>(storeDetails.ToString());
                    var lstProductDetails = JsonConvert.DeserializeObject<List<FavoritesDTO>>(productDetails.ToString());
                    var lstServiceDetails = JsonConvert.DeserializeObject<List<FavoritesDTO>>(serviceDetails.ToString());
                    if (lstStoreDetails != null)
                    {
                        if (lstStoreDetails.Count != 0)
                        { 
                            AppUtils.WishStorelist = lstStoreDetails; 
                        }

                        else
                            obj.StoreDetailslst = null;
                    }
                    if (lstProductDetails != null)
                    {
                        if (lstProductDetails.Count != 0)
                        { 
                            AppUtils.WishProductlist = lstProductDetails; 
                        }
                        else
                            obj.ProductDetailslst = null;
                    }
                    if (lstServiceDetails != null)
                    {
                        if (lstServiceDetails.Count != 0)
                        { 
                            AppUtils.WishServicelist = lstServiceDetails; 

                        }
                        else
                            obj.ServiceDetailslst = null;
                    }
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                ErrorLogDTO err = new ErrorLogDTO(); 
                _errorHandler.WriteError(err, ex.Message);
                return RedirectToAction("Error");
            }
        }
    }

    [HttpPost]
    public async Task<ActionResult> PVStoresWishlistAsync(int pageNumber, int SortId)
    {
        FavoritesDTO obj = new FavoritesDTO();
        var PageSize = decimal.Parse(_options.PageSize);
        List<FavoritesDTO> list = new List<FavoritesDTO>();
        list = AppUtils.WishStorelist;
        int toc = list != null && list.Count() > 0 ? list.Count : 0;
        int count = toc / Convert.ToInt32(PageSize);
        if (list != null && list.Count() > 0)
            list = pageNumber == 1 ? list.Take(Convert.ToInt32(PageSize)).ToList() : list.Skip(((pageNumber + Convert.ToInt32(PageSize)) - 2)).Take(Convert.ToInt32(PageSize)).ToList();
        obj.StoreDetailslst = list;
        obj.StorePageNumber = pageNumber;
        obj.StorepagingNumber = Convert.ToDecimal(Convert.ToDecimal(toc) / PageSize) > Convert.ToDecimal(Convert.ToInt32(Convert.ToDecimal(toc) / PageSize)) ? Convert.ToInt32((toc / PageSize)) + 1 : Convert.ToInt32((toc / PageSize));

        //return PartialView("PVStoreWishlist", obj);
            var result = await _viewRenderService.RenderToStringAsync("WishList/PVStoreWishlist", obj);
            return Json(new { result = result });
        }

    [HttpPost]
    public async Task<ActionResult> PVProductsWishlistAsync(int pageNumber, int SortId)
    {

        FavoritesDTO obj = new FavoritesDTO();
        var PageSize = decimal.Parse(_options.PageSize);
        List<FavoritesDTO> list = new List<FavoritesDTO>();
        list = AppUtils.WishProductlist;
        int toc = list != null && list.Count() > 0 ? list.Count : 0;
        int count = toc / Convert.ToInt32(PageSize);
        if (list != null && list.Count() > 0)
            list = pageNumber == 1 ? list.Take(Convert.ToInt32(PageSize)).ToList() : list.Skip(((pageNumber + Convert.ToInt32(PageSize)) - 2)).Take(Convert.ToInt32(PageSize)).ToList();
        obj.ProductDetailslst = list;
        obj.StorePageNumber = pageNumber;
        obj.StorepagingNumber = Convert.ToDecimal(Convert.ToDecimal(toc) / PageSize) > Convert.ToDecimal(Convert.ToInt32(Convert.ToDecimal(toc) / PageSize)) ? Convert.ToInt32((toc / PageSize)) + 1 : Convert.ToInt32((toc / PageSize));
        //return PartialView("PVProductWishlist", obj);

            var result = await _viewRenderService.RenderToStringAsync("WishList/PVProductWishlist", obj);
            return Json(new { result = result });
        }
    [HttpPost]
    public async Task<ActionResult> PVServicesWishlistAsync(int pageNumber, int SortId)
    {

        FavoritesDTO obj = new FavoritesDTO();
        var PageSize = decimal.Parse(_options.PageSize);
        List<FavoritesDTO> list = new List<FavoritesDTO>();
        list = AppUtils.WishServicelist;
        int toc = list != null && list.Count() > 0 ? list.Count : 0;
        int count = toc / Convert.ToInt32(PageSize);
        if (list != null && list.Count() > 0)
            list = pageNumber == 1 ? list.Take(Convert.ToInt32(PageSize)).ToList() : list.Skip(((pageNumber + Convert.ToInt32(PageSize)) - 2)).Take(Convert.ToInt32(PageSize)).ToList();
        obj.ServiceDetailslst = list;
        obj.StorePageNumber = pageNumber;
        obj.StorepagingNumber = Convert.ToDecimal(Convert.ToDecimal(toc) / PageSize) > Convert.ToDecimal(Convert.ToInt32(Convert.ToDecimal(toc) / PageSize)) ? Convert.ToInt32((toc / PageSize)) + 1 : Convert.ToInt32((toc / PageSize));
            //return PartialView("PVServiceWishlist", obj);
            var result = await _viewRenderService.RenderToStringAsync("WishList/PVServiceWishlist", obj);
            return Json(new { result = result });
        }

    [HttpPost]
    public async Task<ActionResult> AddDeleteFavorite(int Id, int Type, int StatusId, string ItemId, int SubCategoryId = 0)
    {
        var status = "false";
        using (HttpClient client = new HttpClient())
        {
            _commonHeader.setHeaders(client);
            try
            {
                MyOrdersDTO obj = new MyOrdersDTO();
                SessionDTO login = AppUtils.UserLogin; 
                if (login.UserId != 0)
                {  
                    obj.Type = Type;
                    obj.StatusId = StatusId;
                    obj.Ids = ItemId;
                    obj.SubCategoryId = SubCategoryId;
                    obj.UserId = login.UserId;  
                }
                else
                {
                    return Json(new { success = status });
                }

                HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/OrdersAPI/NewAddGetDeleteUserFavorite", obj);
                if (responseMessageViewDocuments.IsSuccessStatusCode)
                {
                    var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                    var docs = JsonConvert.DeserializeObject<MyOrdersDTO>(responseData);
                    var jobj = JObject.Parse(responseData);
                    status = jobj.SelectToken("Status").ToString();

                    if (StatusId == 1)
                    {
                        List<FavoritesDTO> list = new List<FavoritesDTO>();
                        list = (List<FavoritesDTO>)TempData.Peek("WishProductlist");
                        list.RemoveAll(x => x.FavoriteId == Id);
                        AppUtils.WishProductlist = list;
                    }
                    else if (StatusId == 2)
                    {
                        List<FavoritesDTO> list = new List<FavoritesDTO>();
                        list = (List<FavoritesDTO>)TempData.Peek("WishServicelist");
                        list.RemoveAll(x => x.FavoriteId == Id);
                        AppUtils.WishServicelist = list;
                    }
                    else if (StatusId == 3)
                    {
                        List<FavoritesDTO> list = new List<FavoritesDTO>();
                        list = (List<FavoritesDTO>)TempData.Peek("WishStorelist");
                        list.RemoveAll(x => x.FavoriteId == Id);
                        AppUtils.WishStorelist = list;

                    }
                     
                }
                return Json(new { success = status });
            }
            catch (Exception ex)
            {
                return Json(new { success = status });
            }
        }
    }
}
    
}
