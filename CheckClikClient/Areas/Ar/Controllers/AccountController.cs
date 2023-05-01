using CheckClickClient.Models;
using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Options; 
using CheckClikClient.Models;
using CheckClickClient;
using CheckClikClient;

namespace Customer.Areas.Ar.Controllers
{
    [Area("ar")]
    [Route("ar")]
    public class AccountController : Controller
    {
        string currentUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;
        private readonly IViewRenderService _viewRenderService;

        public AccountController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService)
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

        public IActionResult Login()
        {
            return View(new UsersDTO());
        }

        //[HttpPost]
        //public async Task<IActionResult> LoginAsync(UsersDTO obj)
        //{
        //    //authenticateRequest.UserType = "1";
        //    //UsersDTO authResponse = new UsersDTO();
        //    //authResponse = _HealthCareAPI.AuthenticateAsync(authenticateRequest).Result;

        //    //if (string.IsNullOrEmpty(authResponse.Token))
        //    //{
        //    //    TempData["message"] = authResponse.Message;
        //    //    return View();
        //    //}
        //    //else
        //    //{
        //    //    AppUtils.AuthenticationResponse = authResponse;
        //    //    return RedirectToAction("Dashboard", "Home");
        //    //}

        //    using (HttpClient client = new HttpClient())
        //    {
        //        _commonHeader.setHeaders(client);
        //        try
        //        {
        //            obj.Status = true;
        //            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/UsersAPI/UserLogin", obj);
        //            if (responseMessage.IsSuccessStatusCode)
        //            {
        //                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
        //                var org = JsonConvert.DeserializeObject<UsersDTO>(responseData);

        //                if (Convert.ToInt32(org.Id) > 0)
        //                {
        //                    AppUtils.AdminLogin = new UsersDTO() { Id = org.Id, EmpID = org.EmpID, FullName = org.FullName, EmailId = org.EmailId, RoleId = org.RoleId, RoleNameEn = org.RoleNameEn };
        //                    obj = new UsersDTO();
        //                    obj.RoleId = org.RoleId;
        //                    HttpResponseMessage responseMessage2 = await client.PostAsJsonAsync("api/UsersAPI/NewGetUserMenu", obj);
        //                    if (responseMessage2.IsSuccessStatusCode)
        //                    {
        //                        var responseData2 = responseMessage2.Content.ReadAsStringAsync().Result;
        //                        var response = JObject.Parse(responseData2);
        //                        var mainstr = response.SelectToken("MainCategory").ToString();
        //                        var substr = response.SelectToken("SubCategory").ToString();
        //                        var MainCategory = JsonConvert.DeserializeObject<List<AdminMenuCategoryDTO>>(mainstr);
        //                        var SubCategory = JsonConvert.DeserializeObject<List<AdminMenuCategoryDTO>>(substr);
        //                        AppUtils.MenuMainCategory = MainCategory;
        //                        AppUtils.SubMainCategory = SubCategory;
        //                    }
        //                    else
        //                    {
        //                        AppUtils.MenuMainCategory = new List<AdminMenuCategoryDTO>();
        //                        AppUtils.SubMainCategory = new List<AdminMenuCategoryDTO>();

        //                        //Session["MenuMainCategory"] = new MainCategoryDTO();
        //                        //Session["SubMainCategory"] = new MainCategoryDTO();
        //                    }
        //                    //TempData["Success"] = "Login Successful";
        //                    return RedirectToAction("Index", "Dashboard");
        //                }
        //                else
        //                {
        //                    TempData["Error"] = "Invalid Login! please try again.";
        //                }
        //            }

        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLogDTO err = new ErrorLogDTO();
        //            AssignValuesToDTO.AssingDToValues(err, ex, "LoginController/IndexPost", "India Standard Time.");
        //            _errorHandler.WriteError(err, ex.Message);
        //            return RedirectToAction("Error");
        //        }
        //    }
        //}
    }
}
