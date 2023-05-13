using CheckClickClient;
using CheckClikClient.Handlers;
using Customer.Models;
using Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using CheckClickClient.Models;
using SearchLibrary.Models;
using System.Collections.Generic;
using CheckClikClient;

namespace Customer.Areas.Ar.Controllers
{
    [Area("ar")]
    [Route("ar")]
    public class UserController : BaseController
    {
        string currentUrl = "";
        string baseUrl = "";
        string Profileurl = "";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;
        private readonly IViewRenderService _viewRenderService;

        public UserController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService) : base(httpContextAccessor, errorHandler, options, commonHeader)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options.Value;

            var baseURL = _options.baseUrl;// System.Configuration.ConfigurationManager.AppSettings["baseUrl"].ToString().ToLower();
            currentUrl = _httpContextAccessor.HttpContext.Request.PathBase.ToString();//System.Web.HttpContext.Current.Request.Url.ToString().ToLower();// Current.Request.Url.AbsolutePath;
            currentUrl = currentUrl.Replace(baseURL, baseURL + "ar/");
            Profileurl = _options.profileUrl;

            ViewBag.CurrentURL = currentUrl;
            _errorHandler = errorHandler;
            _commonHeader = commonHeader;
            _viewRenderService = viewRenderService;
        }

        [Route("user/index")]
        public ActionResult Index(string returnurl = "")
        {
            if (!string.IsNullOrEmpty(returnurl))
            {
                if (AppUtils.UserLogin != null && AppUtils.UserLogin.IsLogin == true)
                {
                    return Redirect(returnurl);
                }
                TempData["ReturnUrl"] = returnurl;
            }
            else
            {
                if (AppUtils.UserLogin != null && AppUtils.UserLogin.IsLogin == true)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }

        [Route("user/indexnest")]
        public ActionResult IndexNest(string returnurl = "")
        {
            if (!string.IsNullOrEmpty(returnurl))
            {
                if (AppUtils.UserLogin != null && AppUtils.UserLogin.IsLogin == true)
                {
                    return Redirect(returnurl);
                }
                TempData["ReturnUrl"] = returnurl;
            }
            else
            {
                if (AppUtils.UserLogin != null && AppUtils.UserLogin.IsLogin == true)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }
         
        [HttpPost]
        public async Task<ActionResult> UserLoginValidation(UserRegistrationDTO UserData)
        {
            //UserData.LoginName = "+966" + UserData.MobileNo;
            // UserRegistrationDTO user = JsonConvert.DeserializeObject<UserRegistrationDTO>(userData.ToString());
            ResponseDTO resp = new ResponseDTO();
            resp.Status = false;
            resp.Message = "Something went wrong";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewUserValidation", UserData);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        if (Status)
                        {
                            var Data = response.SelectToken("Data");
                            UserData = JsonConvert.DeserializeObject<UserRegistrationDTO>(Data.ToString());
                            if (UserData.Id > 0)
                            {
                                //SessionDTO objs = (SessionDTO)AppUtils.UserLogin;
                                SessionDTO objs = AppUtils.UserLogin;
                                if (objs == null)
                                {
                                    SessionDTO ss = new SessionDTO();
                                    ss.UserId = UserData.Id;
                                    ss.UserName = UserData.FirstName;
                                    ss.IsLogin = true;
                                    ss.UserProfile = UserData.ProfilePhoto == "" || UserData.ProfilePhoto == null ? Profileurl + "user.png" : Profileurl + UserData.ProfilePhoto;
                                    ss.UserChatId = UserData.UserChatId;
                                    ss.UserGroupChatId = UserData.JGroupChatId.ToString();
                                    AppUtils.UserLogin = ss;
                                    AppUtils.CityList = new List<CityDTO>();
                                }
                                else
                                {
                                    objs.IsLogin = true;
                                    objs.UserId = UserData.Id;
                                    objs.UserName = UserData.FirstName;
                                    //objs.UserProfile = Profileurl + UserData.ProfilePhoto;
                                    objs.UserProfile = UserData.ProfilePhoto == "" || UserData.ProfilePhoto == null ? Profileurl + "user.png" : Profileurl + UserData.ProfilePhoto;
                                    objs.UserChatId = UserData.UserChatId;
                                    objs.UserGroupChatId = UserData.JGroupChatId.ToString();
                                    AppUtils.UserLogin = objs;
                                    AppUtils.CityList = new List<CityDTO>();
                                }

                            }
                            resp.Status = Status;
                            resp.Message = Message;
                            if (TempData.Peek("ReturnUrl") != null && TempData.Peek("ReturnUrl") != "")
                            {
                                resp.ReturnUrl = (string)TempData.Peek("ReturnUrl");
                            }

                        }
                        else
                        {
                            var Data = response.SelectToken("Data");
                            UserData = JsonConvert.DeserializeObject<UserRegistrationDTO>(Data.ToString());
                            AppUtils.UserId = null;
                            //resp.Status = UserData.StatusCode == "1" ? true : false;
                            resp.Status = Status;
                            resp.Message = Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ErrorLogDTO err = new ErrorLogDTO();
                    //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                    //ErrorHandler.WriteError(err, ex.Message);
                }
            }
            return Json(resp);

        }

        [Route("user/signup")]
        public ActionResult SignUp()
        { 
            return View();
        }

        [Route("User/SignUpNest")]
        public ActionResult SignUpNest()
        { 
            return View();
        }

        [Route("User/Registration")]

        [HttpPost]
        public async Task<ActionResult> Registration(string userData)
        {

            UserRegistrationDTO user = JsonConvert.DeserializeObject<UserRegistrationDTO>(userData.ToString());
            user.MobileNo = "+966" + user.MobileNo;
            user.FlagId = 1;
            ResponseDTO resp = new ResponseDTO();
            resp.Status = false;
            resp.Message = "Something went wrong";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    // branch = JsonConvert.DeserializeObject<BranchDTO>(BasicInfoJson.ToString());
                    //  branch.CreatedBy = login.UserId;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewUserRegistration", user);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        user = JsonConvert.DeserializeObject<UserRegistrationDTO>(Data.ToString());

                        //resp.Status = user.StatusCode == "1" ? true : false;
                        //resp.Message = user.StatusMessage;
                        resp.Status = Status;
                        //Hussain Login after successfull registration
                        if (resp.Status == true)
                        {
                            HttpResponseMessage responseMessageL = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewUserValidation", user);
                            if (responseMessageL.IsSuccessStatusCode)
                            {
                                var responseDataL = responseMessage.Content.ReadAsStringAsync().Result;
                                var responseL = JObject.Parse(responseDataL);
                                bool StatusL = Convert.ToBoolean(responseL.SelectToken("Status"));
                                var MessageL = Convert.ToString(responseL.SelectToken("Message"));
                                if (StatusL)
                                {
                                    var DataL = responseL.SelectToken("Data");
                                    user = JsonConvert.DeserializeObject<UserRegistrationDTO>(Data.ToString());
                                    if (user.Id > 0)
                                    {

                                        SessionDTO objs = AppUtils.UserLogin;
                                        if (objs == null)
                                        {
                                            SessionDTO ss = new SessionDTO();
                                            ss.UserId = user.Id;
                                            ss.UserName = user.FirstName;
                                            ss.IsLogin = true;
                                            ss.CRN = user.CRN;
                                            //ss.UserProfile = Profileurl + user.ProfilePhoto;
                                            ss.UserProfile = user.ProfilePhoto == "" || user.ProfilePhoto == null ? Profileurl + "user.png" : Profileurl + user.ProfilePhoto;
                                            ss.UserChatId = user.UserChatId;
                                            AppUtils.UserLogin = ss;
                                        }
                                        else
                                        {
                                            objs.IsLogin = true;
                                            objs.UserId = user.Id;
                                            objs.UserName = user.FirstName;
                                            //objs.UserProfile = Profileurl + user.ProfilePhoto;
                                            objs.UserProfile = user.ProfilePhoto == "" || user.ProfilePhoto == null ? Profileurl + "user.png" : Profileurl + user.ProfilePhoto;
                                            objs.UserChatId = user.UserChatId;
                                            AppUtils.UserLogin = objs;
                                        }

                                    }
                                    //resp.Status = Status;
                                    //resp.Message = Message;
                                }
                            }
                        }
                        resp.Message = Message;
                    }
                }
                catch (Exception ex)
                { 
                }

            }
            return Json(resp);

        }


        [Route("User/GetOtp")]
        [HttpGet]
        public async Task<ActionResult> GetOtp(string MobileNo)
        {
            MessageOTPDTO msgOtp = new MessageOTPDTO();
            msgOtp.MobileNo = "+966" + MobileNo;
            msgOtp.FlagId = 1;
            ResponseDTO resp = new ResponseDTO();
            resp.Status = false;
            resp.Message = "Something went wrong";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewGetMobileOTP", msgOtp);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        msgOtp = JsonConvert.DeserializeObject<MessageOTPDTO>(Data.ToString());
                        resp.ValidTime = msgOtp.ValidUntil.ToLongDateString();
                        //resp.Status = msgOtp.StatusCode == "1" ? true : false;
                        resp.Status = Status;
                        resp.Message = Message;
                        // omsgOtprg.Message = org.Id.ToString();
                        resp.ValidTime = DateTime.Now.AddMinutes(10).ToString("dd-MM-yyyy h:mm:ss tt");
                    }
                }
                catch (Exception ex)
                {
                    //ErrorLogDTO err = new ErrorLogDTO();
                    //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                    //ErrorHandler.WriteError(err, ex.Message);
                }
            }

            return Json(resp);
        }


        [HttpGet]
        public async Task<ActionResult> GetUpdateMobile(string MobileNo, string otp)
        {
            MessageOTPDTO msgOtp = new MessageOTPDTO();
            msgOtp.MobileNo = "+966" + MobileNo;
            msgOtp.FlagId = 1;
            msgOtp.OTPCode = otp;
            SessionDTO objs = AppUtils.UserLogin;
            msgOtp.Id = Convert.ToInt32(objs.UserId);
            ResponseDTO resp = new ResponseDTO();
            resp.Status = false;
            resp.Message = "Something went wrong";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewUpdateMobile", msgOtp);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        msgOtp = JsonConvert.DeserializeObject<MessageOTPDTO>(Data.ToString());
                        resp.ValidTime = msgOtp.ValidUntil.ToLongDateString();
                        resp.Status = Status;
                        resp.Message = Message;
                         
                    }
                }
                catch (Exception ex)
                { 
                }
            }

            return Json(resp);
        }

        [HttpGet]
        public async Task<ActionResult> GetUpdateEmail(string vefEmail)
        {
            MessageOTPDTO msgOtp = new MessageOTPDTO();
            //msgOtp.MobileNo = "+966" + MobileNo;
            msgOtp.EmailId = vefEmail;
            msgOtp.FlagId = 2;
            //msgOtp.OTPCode = otp;
            SessionDTO objs = AppUtils.UserLogin;
            msgOtp.Id = Convert.ToInt32(objs.UserId);
            ResponseDTO resp = new ResponseDTO();
            resp.Status = false;
            resp.Message = "Something went wrong";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewUpdateEmail", msgOtp);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        msgOtp = JsonConvert.DeserializeObject<MessageOTPDTO>(Data.ToString());
                        resp.ValidTime = msgOtp.ValidUntil.ToLongDateString();
                        resp.Status = Status;
                        resp.Message = Message;

                        //resp.Status = msgOtp.StatusCode == "1" ? true : false;
                        //resp.Message = msgOtp.StatusMessage;
                        // omsgOtprg.Message = org.Id.ToString();
                    }
                }
                catch (Exception ex)
                {
                    //ErrorLogDTO err = new ErrorLogDTO();
                    //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                    //ErrorHandler.WriteError(err, ex.Message);
                }
            }

            return Json(resp);
        }



        [Route("User/GetVerifyEmail")]
        [HttpGet]
        public async Task<JsonResult> GetVerifyEmail(string Email)
        {
            MessageOTPDTO msgOtp = new MessageOTPDTO();
            msgOtp.EmailId = Email;
            msgOtp.FlagId = 4;
            ResponseDTO resp = new ResponseDTO();
            resp.Status = false;
            resp.Message = "Something went wrong";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewVerifyEmail", msgOtp);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        msgOtp = JsonConvert.DeserializeObject<MessageOTPDTO>(Data.ToString());
                        resp.ValidTime = msgOtp.ValidUntil.ToLongDateString(); 

                        resp.Status = Status;
                        resp.Message = Message; 
                    }
                }
                catch (Exception ex)
                { 
                }
            }

            return Json(resp);
        }


        [Route("User/GetForgotVerifyMobile")]
        [HttpGet]
        public async Task<ActionResult> GetForgotVerifyMobile(string MobileNo)
        {
            MessageOTPDTO msgOtp = new MessageOTPDTO();
            msgOtp.MobileNo = "+966" + MobileNo;
            msgOtp.FlagId = 2;
            ResponseDTO resp = new ResponseDTO();
            resp.Status = false;
            resp.Message = "Something went wrong";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewGetMobileOTP", msgOtp);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        msgOtp = JsonConvert.DeserializeObject<MessageOTPDTO>(Data.ToString());
                        resp.ValidTime = msgOtp.ValidUntil.ToLongDateString();
                        resp.Status = Status;
                        resp.Message = Message;

                        //resp.Status = msgOtp.StatusCode == "1" ? true : false;
                        //resp.Message = msgOtp.StatusMessage;
                        // omsgOtprg.Message = org.Id.ToString();
                    }
                }
                catch (Exception ex)
                {
                    //ErrorLogDTO err = new ErrorLogDTO();
                    //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                    //ErrorHandler.WriteError(err, ex.Message);
                }
            }

            return Json(resp);
        }

        [HttpGet]
        [Route("User/GetForgotVerifyOtp")]
        public async Task<ActionResult> GetForgotVerifyOtp(string MobileNo, string otp)
        {
            MessageOTPDTO msgOtp = new MessageOTPDTO();
            msgOtp.MobileNo = "+966" + MobileNo;
            msgOtp.OTPCode = otp;
            msgOtp.FlagId = 5;
            ResponseDTO resp = new ResponseDTO();
            resp.Status = false;
            resp.Message = "Something went wrong";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewVerifyForgotPasswordOTP", msgOtp);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        msgOtp = JsonConvert.DeserializeObject<MessageOTPDTO>(Data.ToString());
                        resp.ValidTime = msgOtp.ValidUntil.ToLongDateString();
                        resp.Status = Status;
                        resp.Message = Message;

                        //resp.Status = msgOtp.StatusCode == "1" ? true : false;
                        //resp.Message = msgOtp.StatusMessage;
                        // omsgOtprg.Message = org.Id.ToString();
                    }
                }
                catch (Exception ex)
                {
                    //ErrorLogDTO err = new ErrorLogDTO();
                    //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                    //ErrorHandler.WriteError(err, ex.Message);
                }
            }

            return Json(resp);
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePassword(string userData)
        {
            UserRegistrationDTO user = JsonConvert.DeserializeObject<UserRegistrationDTO>(userData.ToString());
            user.MobileNo = "+966" + user.MobileNo;
            user.FlagId = 2;
            user.Id = int.Parse(user.OtpCode);
            ResponseDTO resp = new ResponseDTO();
            resp.Status = false;
            resp.Message = "Something went wrong";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    // branch = JsonConvert.DeserializeObject<BranchDTO>(BasicInfoJson.ToString());
                    //  branch.CreatedBy = login.UserId;
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewSetNewPassword", user);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");
                        var login = new UserRegistrationDTO() { LoginName = user.MobileNo, Password = user.Password };
                        user = JsonConvert.DeserializeObject<UserRegistrationDTO>(Data.ToString());
                        resp.Status = Status;
                        resp.Message = Message;
                        //resp.Status = user.StatusCode == "1" ? true : false;
                        //resp.Message = user.StatusMessage;

                        if (Status)
                        {
                            HttpResponseMessage responseMessage1 = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewUserValidation", login);
                            if (responseMessage1.IsSuccessStatusCode)
                            {
                                var responseData1 = responseMessage1.Content.ReadAsStringAsync().Result;
                                var response1 = JObject.Parse(responseData1);
                                bool Status1 = Convert.ToBoolean(response.SelectToken("Status"));
                                var Message1 = Convert.ToString(response.SelectToken("Message"));
                                if (Status1)
                                {
                                    var Data1 = response.SelectToken("Data");
                                    var UserData1 = JsonConvert.DeserializeObject<UserRegistrationDTO>(Data.ToString());
                                    if (UserData1.Id > 0)
                                    {
                                        SessionDTO objs = AppUtils.UserLogin;
                                        if (objs == null)
                                        {
                                            SessionDTO ss = new SessionDTO();
                                            ss.UserId = UserData1.Id;
                                            ss.UserName = UserData1.FirstName;
                                            ss.IsLogin = true;
                                            ss.UserProfile = Profileurl + UserData1.ProfilePhoto;
                                            ss.UserChatId = UserData1.UserChatId;
                                            ss.UserGroupChatId = UserData1.JGroupChatId.ToString();
                                            AppUtils.UserLogin = ss;
                                        }
                                        else
                                        {
                                            objs.IsLogin = true;
                                            objs.UserId = UserData1.Id;
                                            objs.UserName = UserData1.FirstName;
                                            objs.UserProfile = Profileurl + UserData1.ProfilePhoto;
                                            objs.UserChatId = UserData1.UserChatId;
                                            objs.UserGroupChatId = UserData1.JGroupChatId.ToString();
                                            AppUtils.UserLogin = objs;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ErrorLogDTO err = new ErrorLogDTO();
                    //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                    //ErrorHandler.WriteError(err, ex.Message);
                }

            }
            return Json(resp);

        }




        public async Task<ActionResult> MyProfile()
        {
            CustomerRegistrationDTO user = new CustomerRegistrationDTO();
            if (AppUtils.UserLogin != null)
            {
                SessionDTO objs = AppUtils.UserLogin;
                if (objs.IsLogin == false)
                    return RedirectToAction("index", "user");
                user.FlagId = 1;
                user.Id = Convert.ToInt64(objs.UserId);
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewViewProfile", user);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            var Data = response.SelectToken("Data");
                            user = JsonConvert.DeserializeObject<CustomerRegistrationDTO>(Data.ToString());
                            if (user.ProfilePhoto != null)
                            {
                                TempData["ProfilePhoto"] = user.ProfilePhoto;
                                // user.ProfilePhoto = Profileurl + user.ProfilePhoto;
                                user.ProfilePhoto = user.ProfilePhoto == "" || user.ProfilePhoto == null ? Profileurl + "user.png" : Profileurl + user.ProfilePhoto;
                                user.MobileNo = user.MobileNo.Substring(4);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //ErrorLogDTO err = new ErrorLogDTO();
                        //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                        //ErrorHandler.WriteError(err, ex.Message);
                    }

                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }
        public async Task<ActionResult> MyProfileNest()
        {
            CustomerRegistrationDTO user = new CustomerRegistrationDTO();
            if (AppUtils.UserLogin != null)
            {
                SessionDTO objs = AppUtils.UserLogin;
                if (objs.IsLogin == false)
                    return RedirectToAction("index", "user");
                user.FlagId = 1;
                user.Id = Convert.ToInt64(objs.UserId);
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewViewProfile", user);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            var Data = response.SelectToken("Data");
                            user = JsonConvert.DeserializeObject<CustomerRegistrationDTO>(Data.ToString());
                            if (user.ProfilePhoto != null)
                            {
                                TempData["ProfilePhoto"] = user.ProfilePhoto;
                                // user.ProfilePhoto = Profileurl + user.ProfilePhoto;
                                user.ProfilePhoto = user.ProfilePhoto == "" || user.ProfilePhoto == null ? Profileurl + "user.png" : Profileurl + user.ProfilePhoto;
                                user.MobileNo = user.MobileNo.Substring(4);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //ErrorLogDTO err = new ErrorLogDTO();
                        //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                        //ErrorHandler.WriteError(err, ex.Message);
                    }

                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> MyProfile(CustomerManageAddressDTO user, IFormFile ProfilePic, string optradio, string submit)
        {
            if (submit != "Add" && submit != "Update")
            {
                if (AppUtils.UserLogin != null)
                {
                    CustomerRegistrationDTO customer = new CustomerRegistrationDTO();

                    SessionDTO objs = AppUtils.UserLogin;
                    customer.FirstName = user.FirstName;
                    customer.LastName = user.LastName;
                    customer.Address = user.Address;
                    customer.MobileNo = "+966" + user.MobileNo;
                    customer.FlagId = 2;
                    customer.Id = Convert.ToInt64(objs.UserId);
                    customer.DeviceToken = "afadsf";
                    customer.DeviceVersion = "afadfadfdf";
                    customer.DeviceType = "Web";
                    customer.Language = "En";
                    if (ProfilePic != null)
                    {
                        //string logoguid = Guid.NewGuid().ToString();
                        //var logoname = logoguid + ProfilePic.FileName;
                        //Stream strm = ProfilePic.InputStream;
                        //byte[] bytes = ReadFully(strm);
                        //customer.PhotoBytes = Convert.ToBase64String(bytes);
                        //customer.ProfilePhoto = logoname;
                        //customer.ProfilePhoto1 = null;
                    }
                    else
                    {
                        customer.ProfilePhoto = Convert.ToString(TempData["ProfilePhoto"]);
                    }
                    using (HttpClient client = new HttpClient())
                    {
                        _commonHeader.setHeaders(client);
                        try
                        {
                            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewUpdateProfile", customer);
                            if (responseMessage.IsSuccessStatusCode)
                            {
                                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                                var response = JObject.Parse(responseData);
                                bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                                var Message = Convert.ToString(response.SelectToken("Message"));
                                var Data = response.SelectToken("Data");
                                if (Status == true)
                                {
                                    objs.UserName = user.FirstName;
                                    objs.UserProfile = Profileurl + customer.ProfilePhoto;
                                    AppUtils.UserLogin = objs;

                                }
                                //user = JsonConvert.DeserializeObject<CustomerRegistrationDTO>(Data.ToString());
                                //user.ProfilePhoto = Profileurl + user.ProfilePhoto;
                                //resp.Status = user.StatusCode == "1" ? true : false;
                                //resp.Message = user.StatusMessage;
                            }
                        }
                        catch (Exception ex)
                        {
                            //ErrorLogDTO err = new ErrorLogDTO();
                            //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                            //ErrorHandler.WriteError(err, ex.Message);
                        }
                    }

                }
                else
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("ManageAddressNest");
            }
            else
            {
                if (AppUtils.UserLogin != null)
                {
                    SessionDTO objs = (SessionDTO)AppUtils.UserLogin;
                    switch (submit)
                    {
                        case "Add":
                            user.FlagId = 1;
                            break;
                        case "Update":
                            user.FlagId = 2;
                            user.Id = Convert.ToInt64(user.Id);
                            break;
                    }
                    user.PhoneNumber = "+966" + user.PhoneNumber;


                    user.UserId = Convert.ToInt64(objs.UserId);
                    user.AddressType = Convert.ToInt64(optradio);
                    using (HttpClient client = new HttpClient())
                    {
                        _commonHeader.setHeaders(client);
                        try
                        {
                            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewAddUpdateCustomerAddress", user);
                            if (responseMessage.IsSuccessStatusCode)
                            {
                                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                                var response = JObject.Parse(responseData);
                                bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                                var Message = Convert.ToString(response.SelectToken("Message"));
                                var Data = response.SelectToken("Data");
                                TempData["AddressUpdate"] = Status;
                            }
                        }
                        catch (Exception ex)
                        {
                            //ErrorLogDTO err = new ErrorLogDTO();
                            //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                            //ErrorHandler.WriteError(err, ex.Message);
                        }
                    }

                }
                else
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("ManageAddressNest");
            }

        }

        public class ParamPvOffer {
            public int CityId { get; set; }
            public int DistrictId { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
        }

        [HttpPost]
        public async Task<JsonResult> PVOffer(string model /*int CityId = 0, int DistrictId = 0, string Latitude = "", string Longitude = ""*/)
        {
            ParamPvOffer paramPvOffer = JsonConvert.DeserializeObject<ParamPvOffer>(model);
            int CityId = paramPvOffer.CityId; int DistrictId = paramPvOffer.DistrictId; string Latitude = paramPvOffer.Latitude; string Longitude = paramPvOffer.Longitude;
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

                        obj.ProductListOffers = ProductOfferList;
                        obj.ServiceListOffers = ServiceOfferList;
                        //TempData["ProductOfferList"] = ProductOfferList;
                        //TempData["ServiceOfferList"] = ServiceOfferList;

                        var lstoffers = JsonConvert.DeserializeObject<List<OffersCDTO>>(SubCategory).ToList();
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

                        List<OffersCDTO> lstOffersDto = new List<OffersCDTO>();
                        for (int i = 0; i <= 2; i++)
                        {
                            OffersCDTO offersCDTO = new OffersCDTO();
                            offersCDTO.NameEn = "This is a test offer"; 
                            lstOffersDto.Add(offersCDTO);
                        }
                        obj.ListOffers = lstOffersDto;
                    }
                    else
                    {
                        obj.listStores = new List<StoreDTO>();
                        obj.ListOffers = new List<OffersCDTO>();
                    }
                }
                //return PartialView("_PVOffers", obj);
                obj.ApiURL = _options.apiurl;
                var result = await _viewRenderService.RenderToStringAsync("User/_PVOffersNest", obj);
                return Json(new { result = result });
            }
            catch (Exception ex)
            {
                return Json(new { });
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
         
        public async Task<ActionResult> ManageAddress()
        {
            CustomerManageAddressDTO user = new CustomerManageAddressDTO();
            if (AppUtils.UserLogin != null)
            {

                var userLogin = (Customer.Models.SessionDTO)AppUtils.UserLogin;
                if (userLogin.IsLogin == false)
                {
                    return RedirectToAction("index");
                }

                SessionDTO objs = (SessionDTO)AppUtils.UserLogin;
                user.FlagId = 1;
                user.Id = Convert.ToInt64(0);
                user.UserId = Convert.ToInt64(objs.UserId);
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetCustomerAddress", user);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            var Data = response.SelectToken("Data");
                            IEnumerable<CustomerManageAddressDTO> objbids = new List<CustomerManageAddressDTO>();
                            if (Data != null)
                            {
                                user.ListOfCustomerAddress = JsonConvert.DeserializeObject<List<CustomerManageAddressDTO>>(Data.ToString());
                                objbids = user.ListOfCustomerAddress;
                            }

                            List<CountryDTO> getCountryList = new List<CountryDTO>();
                            CountryDTO obj = new CountryDTO();
                            obj.Id = 0;
                            HttpResponseMessage responseMessageCountry = await client.PostAsJsonAsync("api/CountryAPI/NewGetCountryDetails", obj);
                            if (responseMessageCountry.IsSuccessStatusCode)
                            {
                                var responseDataCountry = responseMessageCountry.Content.ReadAsStringAsync().Result;
                                var DataCountry = JsonConvert.DeserializeObject<List<CountryDTO>>(responseDataCountry);
                                getCountryList = DataCountry;
                            } 
                            if (getCountryList != null)
                            {
                                user.CountryList = getCountryList;
                            }

                            CitysDTO objCity = new CitysDTO();
                            obj.Id = 0;
                            List<CitysDTO> getCityList = new List<CitysDTO>();

                            HttpResponseMessage responseMessageCity = await client.PostAsJsonAsync("api/CityAPI/NewGetCityDetails", obj);
                            if (responseMessageCity.IsSuccessStatusCode)
                            {
                                var responseDataCity = responseMessageCity.Content.ReadAsStringAsync().Result;
                                var DataCity = JsonConvert.DeserializeObject<List<CitysDTO>>(responseDataCity);
                                getCityList = DataCity; 
                            } 
                            if (getCityList != null)
                            {
                                user.CityList = getCityList;
                            } 
                            if (TempData["EditAddress"] != null)
                            {
                                user = JsonConvert.DeserializeObject<CustomerManageAddressDTO>(TempData["EditAddress"].ToString());// (CustomerManageAddressDTO)ViewBag.EditAddress;
                                user.Update = true;
                                ViewBag.optradio = user.AddressType;
                                if (getCountryList != null)
                                {
                                    user.CountryList = getCountryList;
                                }
                                if (getCityList != null)
                                {
                                    user.CityList = getCityList;
                                }
                                user.ListOfCustomerAddress = objbids;
                            }
                            else
                            {
                                user.Create = true;
                            }
                        }
                        else
                        {
                            user.Create = true;
                        }
                    }
                    catch (Exception ex)
                    { 
                    }

                } 
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewViewProfile", new CustomerManageAddressDTO() { Id = Convert.ToInt64(objs.UserId) });
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            var Data = response.SelectToken("Data");
                            var _resUser = JsonConvert.DeserializeObject<CustomerManageAddressDTO>(Data.ToString());
                            if (_resUser.ProfilePhoto != null)
                            {
                                user.FirstName = _resUser.FirstName;
                                user.LastName = _resUser.LastName;
                                user.EmailId = _resUser.EmailId;
                                user.Address = _resUser.Address;

                                TempData["ProfilePhoto"] = _resUser.ProfilePhoto;
                                user.ProfilePhoto = _resUser.ProfilePhoto == "" || _resUser.ProfilePhoto == null ? Profileurl + "user.png" : Profileurl + _resUser.ProfilePhoto;
                                user.MobileNo = _resUser.MobileNo.Substring(4);
                            }
                        }
                    }
                    catch (Exception ex)
                    { 
                    }

                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }


        public async Task<ActionResult> ManageAddressNest()
        {
            CustomerManageAddressDTO user = new CustomerManageAddressDTO();
            if (AppUtils.UserLogin != null)
            {

                var userLogin = (Customer.Models.SessionDTO)AppUtils.UserLogin;
                if (userLogin.IsLogin == false)
                {
                    return RedirectToAction("index");
                }

                SessionDTO objs = (SessionDTO)AppUtils.UserLogin;
                user.FlagId = 1;
                user.Id = Convert.ToInt64(0);
                user.UserId = Convert.ToInt64(objs.UserId);
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetCustomerAddress", user);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            var Data = response.SelectToken("Data");
                            IEnumerable<CustomerManageAddressDTO> objbids = new List<CustomerManageAddressDTO>();
                            if (Data != null)
                            {
                                user.ListOfCustomerAddress = JsonConvert.DeserializeObject<List<CustomerManageAddressDTO>>(Data.ToString());
                                objbids = user.ListOfCustomerAddress;
                            }

                            List<CountryDTO> getCountryList = new List<CountryDTO>();
                            CountryDTO obj = new CountryDTO();
                            obj.Id = 0;
                            HttpResponseMessage responseMessageCountry = await client.PostAsJsonAsync("api/CountryAPI/NewGetCountryDetails", obj);
                            if (responseMessageCountry.IsSuccessStatusCode)
                            {
                                var responseDataCountry = responseMessageCountry.Content.ReadAsStringAsync().Result;
                                var DataCountry = JsonConvert.DeserializeObject<List<CountryDTO>>(responseDataCountry);
                                getCountryList = DataCountry;
                            }
                            if (getCountryList != null)
                            {
                                user.CountryList = getCountryList;
                            }

                            CitysDTO objCity = new CitysDTO();
                            obj.Id = 0;
                            List<CitysDTO> getCityList = new List<CitysDTO>();

                            HttpResponseMessage responseMessageCity = await client.PostAsJsonAsync("api/CityAPI/NewGetCityDetails", obj);
                            if (responseMessageCity.IsSuccessStatusCode)
                            {
                                var responseDataCity = responseMessageCity.Content.ReadAsStringAsync().Result;
                                var DataCity = JsonConvert.DeserializeObject<List<CitysDTO>>(responseDataCity);
                                getCityList = DataCity;
                            }
                            if (getCityList != null)
                            {
                                user.CityList = getCityList;
                            }
                            if (TempData["EditAddress"] != null)
                            {
                                user = JsonConvert.DeserializeObject<CustomerManageAddressDTO>(TempData["EditAddress"].ToString());// (CustomerManageAddressDTO)ViewBag.EditAddress;
                                user.Update = true;
                                ViewBag.optradio = user.AddressType;
                                if (getCountryList != null)
                                {
                                    user.CountryList = getCountryList;
                                }
                                if (getCityList != null)
                                {
                                    user.CityList = getCityList;
                                }
                                user.ListOfCustomerAddress = objbids;
                            }
                            else
                            {
                                user.Create = true;
                            }
                        }
                        else
                        {
                            user.Create = true;
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewViewProfile", new CustomerManageAddressDTO() { Id = Convert.ToInt64(objs.UserId) });
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            var Data = response.SelectToken("Data");
                            var _resUser = JsonConvert.DeserializeObject<CustomerManageAddressDTO>(Data.ToString());
                            if (_resUser.ProfilePhoto != null)
                            {
                                user.FirstName = _resUser.FirstName;
                                user.LastName = _resUser.LastName;
                                user.EmailId = _resUser.EmailId;
                                user.Address = _resUser.Address;

                                TempData["ProfilePhoto"] = _resUser.ProfilePhoto;
                                user.ProfilePhoto = _resUser.ProfilePhoto == "" || _resUser.ProfilePhoto == null ? Profileurl + "user.png" : Profileurl + _resUser.ProfilePhoto;
                                user.MobileNo = _resUser.MobileNo.Substring(4);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageAddress(CustomerManageAddressDTO user, string optradio, string submit)
        {

            if (AppUtils.UserLogin != null)
            {
                SessionDTO objs = (SessionDTO)AppUtils.UserLogin;
                switch (submit)
                {
                    case "Add":
                        user.FlagId = 1;
                        break;
                    case "Update":
                        user.FlagId = 2;
                        user.Id = Convert.ToInt64(user.Id);
                        break;
                }
                user.PhoneNumber = "+966" + user.PhoneNumber;


                user.UserId = Convert.ToInt64(objs.UserId);
                user.AddressType = Convert.ToInt64(optradio);
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewAddUpdateCustomerAddress", user);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            var Data = response.SelectToken("Data");
                            TempData["AddressUpdate"] = Status;
                        }
                    }
                    catch (Exception ex)
                    {
                        //ErrorLogDTO err = new ErrorLogDTO();
                        //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                        //ErrorHandler.WriteError(err, ex.Message);
                    }
                }

            }
            else
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("ManageAddress");
        }

        [Route("user/manageaddressedit")]
        public async Task<ActionResult> ManageAddressEdit(string Id)
        {
            CustomerManageAddressDTO user = new CustomerManageAddressDTO();
            if (AppUtils.UserLogin != null)
            {
                SessionDTO objs = (SessionDTO)AppUtils.UserLogin;
                user.FlagId = 1;
                Id = StringUtil.URLDecrypt(Id);
                user.Id = Convert.ToInt64(Id);
                user.UserId = Convert.ToInt64(objs.UserId);
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetCustomerAddress", user);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            var Data = response.SelectToken("Data");
                            List<CustomerManageAddressDTO> objbids = new List<CustomerManageAddressDTO>();
                            if (Data != null)
                            {
                                foreach (var item in Data)
                                {
                                    CustomerManageAddressDTO objbid = new CustomerManageAddressDTO();
                                    objbid.Id = Convert.ToInt32(item.SelectToken("Id"));
                                    objbid.FullName = Convert.ToString(item.SelectToken("FullName"));
                                    objbid.Address1 = Convert.ToString(item.SelectToken("Address1"));
                                    objbid.Address2 = Convert.ToString(item.SelectToken("Address2"));
                                    objbid.CountryId = Convert.ToInt32(item.SelectToken("CountryId"));
                                    objbid.CityId = Convert.ToInt32(item.SelectToken("CityId"));
                                    objbid.Zipcode = Convert.ToString(item.SelectToken("Zipcode"));
                                    objbid.PhoneNumber = Convert.ToString(item.SelectToken("PhoneNumber")).Substring(4);//Length == 12 ? Convert.ToString(item.SelectToken("PhoneNumber")).Substring(4,11) : Convert.ToString(item.SelectToken("PhoneNumber"));
                                    objbid.AddressType = Convert.ToInt32(item.SelectToken("AddressType"));

                                    objbids.Add(objbid);
                                }
                            }

                            user = objbids[0];

                            //TempData["EditAddress"] = JsonConvert.SerializeObject(user);
                            ViewBag.EditAddress = user;
                            //ViewBag.optradio = user.AddressType;
                        }
                    }
                    catch (Exception ex)
                    {
                        //ErrorLogDTO err = new ErrorLogDTO();
                        //  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                        //ErrorHandler.WriteError(err, ex.Message);
                        return RedirectToAction("manageaddressNest", "user");
                    }

                }
            }
            else
            {
                return RedirectToAction("index");
            }
            user.FlagId = 2;
            TempData["EditAddress"] = JsonConvert.SerializeObject(user);
            return RedirectToAction("ManageAddressNest","User");
        }


        [Route("user/manageaddressdelete")]
        public async Task<ActionResult> ManageAddressDelete(string Id)
        {
            CustomerManageAddressDTO user = new CustomerManageAddressDTO();
            if (AppUtils.UserLogin != null)
            {
                SessionDTO objs = (SessionDTO)AppUtils.UserLogin;
                user.FlagId = 3;
                Id = StringUtil.URLDecrypt(Id);
                user.Id = Convert.ToInt64(Id);
                user.UserId = Convert.ToInt64(objs.UserId);
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewAddUpdateCustomerAddress", user);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            TempData["AddressDelete"] = Status;
                             
                        }
                    }
                    catch (Exception ex)
                    { 
                    }

                }
            }
            else
            {
                return RedirectToAction("index");
            }
            return RedirectToAction("ManageAddressNest");
        }



        [HttpGet]
        public async Task<ActionResult> ChangePassword()
        {
            if (AppUtils.UserLogin == null)
            {
                return RedirectToAction("Index");
            }
            var userLogin = (Customer.Models.SessionDTO)AppUtils.UserLogin;
            if (userLogin.IsLogin == false)
            {
                return RedirectToAction("index");
            }
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> ChangePasswordNest()
        {
            if (AppUtils.UserLogin == null)
            {
                return RedirectToAction("NIndex");
            }
            var userLogin = (Customer.Models.SessionDTO)AppUtils.UserLogin;
            if (userLogin.IsLogin == false)
            {
                return RedirectToAction("NIndex");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            AppUtils.UserLogin = new SessionDTO();
            return RedirectToAction("NIndex", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(string userData)
        {
            CustomerRegistrationDTO user = JsonConvert.DeserializeObject<CustomerRegistrationDTO>(userData.ToString());
            if (AppUtils.UserLogin != null)
            {
                SessionDTO objs = AppUtils.UserLogin;
                user.FlagId = 6;
                user.Id = Convert.ToInt64(objs.UserId);
                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerRegistrationAPI/NewResetPassword", user);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            var Data = response.SelectToken("Data");
                            user.Status = Status;
                            user.Message = Message;
                        }
                    }
                    catch (Exception ex)
                    { 
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
            return Json(user);
        }


    }
}
