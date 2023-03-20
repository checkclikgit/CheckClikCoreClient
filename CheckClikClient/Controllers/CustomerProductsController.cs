using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using CheckClickClient;
using SearchLibrary;

namespace CheckClikClient.Controllers
{
    public class CustomerProductsController : BaseController
    {

        string currentUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpContextAccessor _hhttpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;
        private readonly IViewRenderService _viewRenderService;

        public CustomerProductsController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService) : base(httpContextAccessor, errorHandler, options, commonHeader)
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

        //https://localhost:7057/nawader-amisk/atyab-nawader-almisk-/single/p/VitIf97tGuECYSpY8uFMVGWD9W1653qQP6bI0dv33O5c3iM4sNIGo8fA6Zupm67K
        [Route("{%%%}/{%}/{%%}/p/{Id}")]
        //[Route("{%}/product/details/{id}")]
        public async Task<ActionResult> ProductDetails(string Id)
        {

            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    Id = Id.Replace(" ", "+");
                    Id = Id.Replace("-", "/");
                    Id = Customer.Utils.StringUtil.URLDecrypt(Id);
                    //int Id = 3;
                    string[] data = new string[3];
                    data = Id.Split('_');

                    int BranchId = Convert.ToInt32(data[1]);


                    string[] data2 = data[2].Split('-');
                    int PId = Convert.ToInt32(data2[0]);

                    string[] data3 = data[3].Split('-');
                    int SubCategoryId = Convert.ToInt32(data3[0]);

                    List<ProductDetailsDTO> tolst = new List<ProductDetailsDTO>();


                    await PrepareLayout(BranchId);
                    var viewmoddel = AppUtils.Branch;//(BaseViewModel)TempData["Branch"];
                    //var list = (BranchDTO)TempData.Peek("BrchSubCategory");
                    ViewBag.BranchModel = viewmoddel;


                    // Get store name from route values to validate store name 
                    var routes = this.RouteData.Values;
                    string routestore = Convert.ToString(routes.Values.ElementAt(2));
                    var storename = viewmoddel.StoreNameEn.Replace(" ", "-").ToLower();
                    if (routestore != storename)
                        return RedirectToAction("Index", "Home");

                    TempData["StoreName"] = storename;
                    SessionDTO login = AppUtils.UserLogin;
                    //ProductDetailsDTO obj = new ProductDetailsDTO();
                    ProductDetailsDTO obj = new ProductDetailsDTO();
                    obj.ProductNameEn = data[0];
                    obj.SubCategoryId = SubCategoryId;

                    if (login == null)
                    {
                        //return RedirectToAction("Index", "Products");
                        SessionDTO login1 = new SessionDTO();
                        login1.BranchId = BranchId;
                        login1.StoreId = viewmoddel.StoreId;
                        obj.BranchId = BranchId;
                        login = login1;
                        login1.StoreType = 2;
                        AppUtils.UserLogin = login1;

                    }
                    else
                    {
                        obj.BranchId = BranchId;
                        login.StoreType = 1;
                        login.BranchId = BranchId;
                        login.StoreId = viewmoddel.StoreId;
                        AppUtils.UserLogin = login;

                    }

                    string api_url = _options.apiurl.ToString();
                    string upload_api = _options.UploadLocation.ToString();
                    string folder_name = UploadLocations.Products;
                    obj.ProductInventoryId = PId;
                    if (login.IsLogin == true)
                        obj.UserId = login.UserId;
                    else if
                         (login.IsLogin == false) obj.UserId = 0;

                    ProductDetailsSearchDTO productDetailsSearchDTO = new ProductDetailsSearchDTO();
                    productDetailsSearchDTO.UserId = obj.UserId;
                    productDetailsSearchDTO.ProductInventoryId = obj.ProductInventoryId;

                    //HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/ProductsInfoAPI/NewGetProductsDetailsForCustomers", obj);
                    HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/ProductsInfoAPI/NewGetProductsDetailsForCustomersByProductSearchDetailsDTO", productDetailsSearchDTO);
                    if (responseMessageViewDocuments.IsSuccessStatusCode)
                    {
                        var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;

                        var docs = JsonConvert.DeserializeObject<ProductDetailsDTO>(responseData);
                        var Job = JObject.Parse(responseData);
                        if (docs.Data != null)
                        {
                            var jtok = docs.Data;
                            var jProductListToken = jtok.SelectToken("ProductList");
                            var jTotalRecordsToken = jtok.SelectToken("VariantsList");

                            List<ProductDetailsDTO> Combine_list = new List<ProductDetailsDTO>();
                            List<ProductDetailsDTO> Combine_list_II = new List<ProductDetailsDTO>();
                            List<ProductDetailsDTO> image_lst = new List<ProductDetailsDTO>();

                            obj.ProductDetailsLst = JsonConvert.DeserializeObject<List<ProductDetailsDTO>>(jProductListToken.ToString());
                            //Session.Add("ProductDetailsLst", obj.ProductDetailsLst);
                            AppUtils.ProductDetailsLst = obj.ProductDetailsLst.ToList();
                            //Session.Add("ProductDetailsLst", obj.ProductDetailsLst);
                            obj.VariantsLst = JsonConvert.DeserializeObject<List<ProductDetailsDTO>>(jTotalRecordsToken.ToString());
                        }
                        else
                        {
                            obj.ProductDetailsLst = null;
                            obj.VariantsLst = null;
                        }
                    }
                    obj.UrlPath = api_url + upload_api + folder_name;
                    obj.ProductInventoryId = PId;
                    obj.MinCartQty = obj.ProductDetailsLst.Count() > 0 ? obj.ProductDetailsLst.Where(x => x.ProductInventoryId == PId).FirstOrDefault().MinCartQty : 0;
                    obj.MaxCartQty = obj.ProductDetailsLst.Count() > 0 ? obj.ProductDetailsLst.Where(x => x.ProductInventoryId == PId).FirstOrDefault().MaxCartQty : 0;
                    obj.ProductSkuId = obj.ProductDetailsLst.Count() > 0 ? obj.ProductDetailsLst.Where(x => x.ProductInventoryId == PId).FirstOrDefault().ProductSkuId : "";
                    obj.OrderId = obj.ProductDetailsLst.Count() > 0 ? obj.ProductDetailsLst.Where(x => x.ProductInventoryId == PId).FirstOrDefault().OrderId : 0;
                    if (login.IsLogin == true)
                    {
                        obj.loginval = 1;
                        obj.CartBranchId = login.CartBranchId;
                        obj.BranchId = login.BranchId;

                        IEnumerable<Customer.Models.CartDTO> cartlst = ViewBag.CartList;
                        if (cartlst != null && cartlst.Count() > 0 && cartlst.FirstOrDefault().BranchId == login.CartBranchId)
                        {
                            obj.CartItems = 1;
                        }
                        else
                        {
                            obj.CartItems = 0;
                        }

                    }
                    else
                    {
                        obj.BranchId = login.BranchId;
                        obj.CartBranchId = 0;
                        obj.loginval = 2;
                        obj.CartItems = 0;
                    }
                    return View(obj);
                    return View();
                }
                catch (Exception ex)
                {
                    //ex.Message.ToString();
                    return RedirectToAction("Index", "Products");
                }
            }
        }
        //[Route("{%%%}/{%}/{%%}/p/{Id}")]
        ////[Route("{%}/product/details/{id}")]
        //public async Task<ActionResult> ProductDetails(string Id)
        //{

        //    using (HttpClient client = new HttpClient())
        //    {
        //        _commonHeader.setHeaders(client);
        //        try
        //        { 
        //            return View();
        //        }
        //        catch (Exception ex)
        //        {
        //            //ex.Message.ToString();
        //            return RedirectToAction("Index", "Products");
        //        }
        //    }
        //}
    }
}
