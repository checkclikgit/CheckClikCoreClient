using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using CheckClickClient;
using CheckClikClient.Models;
using SearchLibrary.Models;
//using System.Web.Script.Serialization;

namespace CheckClikClient.Controllers
{
    public class CartController : BaseController
    {
        string currentUrl = "";
        string baseUrl = "";
        string Profileurl = "";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;
        private readonly IViewRenderService _viewRenderService;

        public CartController(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader, IViewRenderService viewRenderService) : base(httpContextAccessor, errorHandler, options, commonHeader)
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

        public async Task<ActionResult> CartPreview()
        {
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    CartDTO cartDtoobj = new CartDTO();
                    //kamrul
                    GetCartItemsDTO obj = new GetCartItemsDTO();
                    SessionDTO _login = AppUtils.UserLogin;
                    if (_login == null)
                        return RedirectToAction("Index", "User", new { returnurl = baseUrl + "cart/cartpreview" });
                    if (_login.IsLogin == false)
                        return RedirectToAction("Index", "User", new { returnurl = baseUrl + "cart/cartpreview" });
                    await PrepareLayout(_login.BranchId);
                    var viewmoddel = (BaseViewModel)TempData["Branch"];
                    var list = (BranchDTO)TempData.Peek("BrchSubCategory");
                    ViewBag.BranchModel = viewmoddel;

                    //ProductListDTO obj = new ProductListDTO();
                    List<CartDTO> tolst = new List<CartDTO>();

                    string api_url =  _options.apiurl;
                    string upload_api = _options.UploadLocation;
                    string folder_name = UploadLocations.Products;
                    obj.BranchId = _login.BranchId;
                    obj.UserId = _login.UserId;
                    cartDtoobj.BranchId = _login.BranchId;
                    cartDtoobj.UserId = _login.UserId;



                    HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/ProductsInfoAPI/NewGetCartItemsKm", obj);
                    if (responseMessageViewDocuments.IsSuccessStatusCode)
                    {
                        var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                        var docs = JsonConvert.DeserializeObject<CartDTO>(responseData);
                        if (docs.Data != null)
                        {
                            var jtok = docs.Data;
                            var jProductListToken = jtok.SelectToken("CartList");
                            if (jProductListToken.ToString() != "[]")
                            {
                                cartDtoobj.CartList = JsonConvert.DeserializeObject<List<CartDTO>>(jProductListToken.ToString());
                                cartDtoobj.CheckOutAmount = cartDtoobj.CartList.FirstOrDefault().CheckOutAmount;
                                cartDtoobj.CouponId = cartDtoobj.CartList.FirstOrDefault().CouponId;
                                cartDtoobj.MaxDiscountAmount = cartDtoobj.CartList.FirstOrDefault().MaxDiscountAmount;
                                cartDtoobj.VoucherDiscount = cartDtoobj.CartList.FirstOrDefault().VoucherDiscount;
                                cartDtoobj.AvailabilityStatus = cartDtoobj.CartList.FirstOrDefault().AvailabilityStatus;
                                cartDtoobj.MinimumOrderValue = cartDtoobj.CartList.FirstOrDefault().MinimumOrderValue;
                                cartDtoobj.TotalItemPrice = cartDtoobj.CartList.FirstOrDefault().TotalItemPrice;
                                cartDtoobj.VatPercentage = cartDtoobj.CartList.FirstOrDefault().VatPercentage;
                                cartDtoobj.DiscountAmount = cartDtoobj.CartList.FirstOrDefault().DiscountAmount;
                                cartDtoobj.SubTotal = cartDtoobj.CartList.FirstOrDefault().SubTotal;
                                cartDtoobj.VAT = cartDtoobj.CartList.FirstOrDefault().VAT;
                                cartDtoobj.GrandTotal = cartDtoobj.CartList.FirstOrDefault().GrandTotal;

                            }
                            else
                                cartDtoobj.CartList = new List<CartDTO>();

                            var jCouponListToken = jtok.SelectToken("CouponList");
                            if (jCouponListToken.ToString() != "[]")
                                cartDtoobj.CouponList = JsonConvert.DeserializeObject<List<CartDTO>>(jCouponListToken.ToString());
                            else
                                cartDtoobj.CouponList = new List<CartDTO>();

                        }
                        else
                            cartDtoobj.CartList = tolst;

                        cartDtoobj.UrlPath = api_url + upload_api + folder_name;
                    }

                    cartDtoobj.BaseUrl_c = _options.baseUrl;
                    return View(cartDtoobj);
                }
                catch (Exception ex)
                {
                    return Json(new SelectList("", "Value", "Text"));
                }
            }
        }

        public async Task<JsonResult> UpdateProductStatus(long CartId, int quantity, int flagid)
        {
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    bool resu = false;
                    //CartDTO obj = new CartDTO();
                    //kamrul
                    UpdateCartDTO obj = new UpdateCartDTO();
                    obj.CartId = CartId;
                    obj.StockQuantity = quantity;
                    obj.FlagId = flagid;

                    //HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/ProductsInfoAPI/NewUpdateCartDetails", obj);
                    //kamrul
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/ProductsInfoAPI/NewUpdateCartDetailsKm", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var check = JsonConvert.DeserializeObject<CartDTO>(responseData);
                        string msg = check.message;
                        if (msg == "1")
                            resu = true;
                        else if (msg == "2")
                            resu = false;
                    }
                    return Json(new { success = resu });
                }
                catch (Exception ex)
                {
                    return Json(new { success = ex });
                }
            }
        }

        public async Task<JsonResult> EditCart(long CouponId, string remarks)
        {
            //if (CouponId == -1)
            //{
            //    //Thread th = new Thread(UpdateRemakrs());
            //    Thread FirstThread = new Thread(() => UpdateRemakrs(remarks));
            //    FirstThread.Start();

            //    return Json(new { success = true, successmessage = "" });
            //}
            bool success = false;
            string successmessage = "";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    AddCouponToCartDTO a = new AddCouponToCartDTO();
                    //CartDTO a = new CartDTO();
                    SessionDTO _login = AppUtils.UserLogin;
                    a.UserId = _login.UserId;
                    a.CouponId = (int)CouponId;
                    a.Remarks = remarks;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewAddCouponToCart", a);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        //var user = JsonConvert.DeserializeObject<CartDTO>(responseData);
                        var json = JObject.Parse(responseData);
                        success = Convert.ToBoolean(json.SelectToken("Status"));
                        successmessage = Convert.ToString(json.SelectToken("Message"));

                    }
                    return Json(new { success = success, successmessage = successmessage });
                }
                catch (Exception ex)
                {
                    return Json("Authenticate", "Authentication");
                }
            }
        }

        public async Task<ActionResult> Checkout()
        {
            ViewBag.siteBaseURL = _options.baseUrl ;
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    GetBranchDetailsForCheckOutDTO dto = new GetBranchDetailsForCheckOutDTO();
                    CartDTO obj = new CartDTO();
                    SessionDTO _login = AppUtils.UserLogin;
                    if (_login == null)
                        return RedirectToAction("Index", "User", new { returnurl = baseUrl + "cart/checkout" });
                    if (_login.IsLogin == false)
                        return RedirectToAction("Index", "User", new { returnurl = baseUrl + "cart/checkout" });
                    await PrepareLayout(_login.BranchId);
                    var viewmoddel = (BaseViewModel)TempData["Branch"];
                    var list = (BranchDTO)TempData.Peek("BrchSubCategory");
                    ViewBag.BranchModel = viewmoddel;

                    List<CartDTO> pslst = new List<CartDTO>();
                    List<CartDTO> dhlst = new List<CartDTO>();

                    obj.BranchId = _login.BranchId;
                    obj.UserId = _login.UserId;
                    dto.BranchId = _login.BranchId;
                    dto.UserId = _login.UserId;


                    CustomerManageAddressDTO user = new CustomerManageAddressDTO();
                    user.FlagId = 1;
                    user.Id = Convert.ToInt64(0);
                    user.UserId = Convert.ToInt64(_login.UserId);

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewGetCustomerAddress", user);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        var Data = response.SelectToken("Data");
                        List<CustomerManageAddressDTO> objbids = new List<CustomerManageAddressDTO>();
                        if (Data != null)
                            obj.ListOfCustomerAddress = JsonConvert.DeserializeObject<List<CustomerManageAddressDTO>>(Data.ToString());
                        else
                            obj.ListOfCustomerAddress = null;

                        //var orderType = response.SelectToken("OrderType");
                        //obj.OrderType =int.Parse(orderType.ToString());
                    }

                    //HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/ProductsInfoAPI/NewGetBranchDetailsForCheckOut", obj);
                    HttpResponseMessage responseMessageViewDocuments = await client.PostAsJsonAsync("api/ProductsInfoAPI/NewGetBranchDetailsForCheckOutKm", dto);
                    if (responseMessageViewDocuments.IsSuccessStatusCode)
                    {
                        var responseData = responseMessageViewDocuments.Content.ReadAsStringAsync().Result;
                        var docs = JsonConvert.DeserializeObject<CartDTO>(responseData);
                        if (docs.Data != null)
                        {
                            var jtok = docs.Data;
                            var jProductListToken = jtok.SelectToken("PickupInServiceWeekdays");
                            pslst = JsonConvert.DeserializeObject<List<CartDTO>>(jProductListToken.ToString());
                            obj.InStoreService = docs.InStoreService;
                            //if (pslst.Count == 0)
                            // commented temporary
                            //    return RedirectToAction("CartPreview", "Cart");
                            int[] pickinstore = new int[pslst.Count()];
                            int i = 0;
                            foreach (var item in pslst)
                            {
                                pickinstore[i] = item.WeekdayId - 1;
                                i++;
                            }
                            //obj.PickupInServiceWeekdays = pickinstore;
                            //obj.PickupInServiceWeekdays = new JavaScriptSerializer().Serialize(pickinstore);
                            obj.PickupInServiceWeekdays = JsonConvert.SerializeObject(pickinstore);
                            var jtokDH = docs.Data;
                            var jProductListTokenDH = jtokDH.SelectToken("DeliveryHomeWeekdays");
                            dhlst = JsonConvert.DeserializeObject<List<CartDTO>>(jProductListTokenDH.ToString());
                            int[] deliveryhome = new int[dhlst.Count()];
                            int j = 0;
                            foreach (var item in dhlst)
                            {
                                deliveryhome[j] = item.WeekdayId - 1;
                                j++;
                            }

                            //if(pslst.Count == 0 || dhlst.Count ==0)
                            //{

                            //obj.DeliveryHomeWeekdays = new JavaScriptSerializer().Serialize(deliveryhome);
                            obj.DeliveryHomeWeekdays = JsonConvert.SerializeObject(deliveryhome);

                            var jCouponListTokenDH = jtokDH.SelectToken("Coupons");
                            obj.CouponList = JsonConvert.DeserializeObject<List<CartDTO>>(jCouponListTokenDH.ToString());

                            var JProductsToken = jtokDH.SelectToken("PoductList");
                            obj.ProductList = JsonConvert.DeserializeObject<List<CartDTO>>(JProductsToken.ToString());
                            if (obj.ProductList.Count() == 0)
                            {
                                TempData["Error"] = "No items available in cart.";
                                return RedirectToAction("CartPreview", "Cart");
                            }

                            obj.PreBookingInStoreTimeUnit = int.Parse(docs.PreBookingInStoreTimeUnit.ToString());
                            obj.PickUpInStoreType = docs.PickUpInStoreType.ToString();
                            obj.valfirst = int.Parse(docs.valfirst.ToString());
                            obj.PreBookingDeliveryTimeUnit = int.Parse(docs.PreBookingDeliveryTimeUnit.ToString());
                            obj.DeliveryHomeService = docs.DeliveryHomeService.ToString();
                            obj.valtwo = int.Parse(docs.valtwo.ToString());
                            obj.Price = Decimal.Parse(docs.TotalAmount.ToString());
                            obj.OrderType = int.Parse(docs.OrderType.ToString());

                            obj.ServiceType = docs.ServiceType.ToString();
                            obj.ServiceProviderName = docs.ServiceProviderName.ToString();
                            obj.TimeSlot = docs.TimeSlot.ToString();
                            obj.CouponId = long.Parse(docs.CouponId.ToString());
                            obj.Remarks = docs.Remarks.ToString();
                            obj.Address = docs.Address.ToString();
                            obj.RequestDate = docs.RequestDate.ToString();
                            obj.ProductNameEn = obj.ProductList.FirstOrDefault().ProductNameEn;
                            obj.InStoreService = docs.InStoreService;
                            obj.IsDelivery = docs.IsDelivery;
                            obj.IsShippingWorldwide = docs.IsShippingWorldwide;
                            obj.ShippingProvider = docs.ShippingProvider;
                            obj.DelDeliveryFee = docs.DelDeliveryFee;
                            obj.ShippingDeliveryFee = docs.ShippingDeliveryFee;
                            obj.ShipmentCODFee = docs.ShipmentCODFee;
                            obj.BranchExpiry = docs.BranchExpiry;
                            obj.BranchExpiryDays = docs.BranchExpiryDays;
                            obj.VatPercentage = docs.VatPercentage;
                            obj.DiscountAmount = docs.DiscountAmount;
                            obj.SubTotal = docs.SubTotal;
                            obj.VAT = docs.VAT;
                            obj.GrandTotal = docs.GrandTotal;
                            obj.MinimumOrderValue = docs.MinimumOrderValue;
                            if ((obj.SubTotal - obj.DiscountAmount) < obj.MinimumOrderValue)
                            {
                                TempData["Error"] = "Minimum Order Value is " + obj.MinimumOrderValue.ToString();
                                return RedirectToAction("CartPreview", "Cart");
                            }

                            if (pslst.Count == 0 && dhlst.Count == 0)
                            {
                                TempData["Error"] = "Order timeslot not available, try after sometime!";
                                return RedirectToAction("CartPreview", "Cart");
                            }

                            //}

                            obj.AvailabilityStatus = docs.AvailabilityStatus;
                            if (obj.AvailabilityStatus == false)
                            {
                                TempData["Error"] = "Store Currently Not Available";
                                return RedirectToAction("CartPreview", "Cart");
                            }


                        }
                    }
                    string api_url = _options.apiurl;
                    string upload_api = _options.UploadLocation;
                    string folder_name = UploadLocations.Products;
                    obj.BranchId = _login.BranchId;
                    obj.UserId = _login.UserId;
                    obj.UrlPath = api_url + upload_api + folder_name;
                    
                    //Temp by kamrul should remove
                    //obj.ListOfCustomerAddress = obj.ListOfCustomerAddress.Take(1);

                    //obj.Price = (Decimal)TempData["TotalAmount"];
                    return View(obj);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Something went wrong, try after sometime.";
                    return RedirectToAction("CartPreview", "Cart");
                }
            }
        }

        public async Task<JsonResult> GetTimeSlots(int WeekdayId, string date, int userid, string type)
        {
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    //CartDTO obj = new CartDTO();
                    //kamrul
                    GetTimeSlotsDTO obj = new GetTimeSlotsDTO();

                    SelectList ddltimeslots = new SelectList("", "SlotId", "TimeSlot", 0);
                    obj.WeekdayId = WeekdayId;
                    obj.Date = Convert.ToDateTime(date);
                    obj.UserId = userid;
                    obj.OrderType = 1;
                    List<CartDTO> _objTimeSlots = new List<CartDTO>();
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/ProductsInfoAPI/NewGetTimeSlotsDayWiseKm", obj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var docs = JsonConvert.DeserializeObject<CartDTO>(responseData);
                        if (docs.Data != null)
                        {
                            var jtok = docs.Data;
                            var jProductListToken = jtok.SelectToken("TimeSlotList");
                            _objTimeSlots = JsonConvert.DeserializeObject<List<CartDTO>>(jProductListToken.ToString());

                            ddltimeslots = new SelectList(_objTimeSlots.Where(x => x.TimingType == type && x.TotalAcceptOrders >= x.Acceptedorders), "SlotId", "TimeSlot", 0);
                        }
                    }
                    return Json(new SelectList(ddltimeslots, "Value", "Text"));
                }
                catch (Exception ex)
                {
                    ErrorLogDTO err = new ErrorLogDTO();
                    _errorHandler.WriteError(err, ex.Message);
                    return Json(new SelectList("", "Value", "Text"));
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveUsersDetails(IFormCollection form)
        {
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    bool status = false;
                    CartDTO obj = new CartDTO();
                    PlaceOrderDTO placeOrderDTO = new PlaceOrderDTO();

                    SessionDTO _login = AppUtils.UserLogin;
                    if (_login == null)
                        return RedirectToAction("IndexNest", "User");
                    if (_login.IsLogin == false)
                        return RedirectToAction("IndexNest", "User");
                    obj.UserId = _login.UserId;
                    obj.Comments = Request.Form["Comments"];
                    obj.AddressId = long.Parse(Request.Form["AddressId"]);
                    obj.PaymentMode = Request.Form["PaymentMode"];
                    obj.PaymentType = Request.Form["PaymentType"];

                    placeOrderDTO.UserId = _login.UserId;
                    placeOrderDTO.Comments = Request.Form["Comments"];
                    placeOrderDTO.AddressId = long.Parse(Request.Form["AddressId"]);
                    placeOrderDTO.PaymentMode = Request.Form["PaymentMode"];
                    placeOrderDTO.PaymentType = Request.Form["PaymentType"];

                    BillingAddressDTO billAddr = new BillingAddressDTO();
                    billAddr.street = Request.Form["street"];
                    billAddr.city = Request.Form["city"];
                    billAddr.region = Request.Form["region"];
                    billAddr.postalcode = Request.Form["postalcode"];
                    billAddr.country = Request.Form["country"];

                    AppUtils.BillingAddr = billAddr;
                    string val = Request.Form["Type"];
                    if (val == "1")
                    {
                        obj.OrderType = int.Parse(Request.Form["OrderType"]);
                        obj.ExpectingDelivery = Convert.ToDateTime(Request.Form["ExpectingDelivery"].ToString());
                        obj.TimeSlotId = long.Parse(Request.Form["TimeSlotId"]);

                        placeOrderDTO.OrderType = int.Parse(Request.Form["OrderType"]);
                        placeOrderDTO.ExpectingDelivery = Convert.ToDateTime(Request.Form["ExpectingDelivery"].ToString());
                        placeOrderDTO.TimeSlotId = long.Parse(Request.Form["TimeSlotId"]);
                    }
                    //Type


                    obj.PaymentStatus = bool.Parse(Request.Form["PaymentStatus"]);
                    placeOrderDTO.PaymentStatus = bool.Parse(Request.Form["PaymentStatus"]);

                    //HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewPlaceOrder", obj);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/OrdersAPI/NewPlaceOrderKm", placeOrderDTO);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        var Data = response.SelectToken("Data");

                        //if (status)
                        //{
                        var check = JsonConvert.DeserializeObject<MyOrdersDTO>(Data.ToString());
                        string msg = check.Message;
                        TempData["OrderStatus"] = check.OrderStatus;

                        return Json(Data.ToString());
                        //}
                        //else
                        //{
                        //    return Json(Data.ToString(), JsonRequestBehavior.AllowGet);
                        //}
                    }
                    else
                    {
                        var result = "2";
                        return Json(result);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new SelectList(ex.ToString(), "Value", "Text"));
                }
            }
        }

        //public  OrderConfirmed(long id, string inv)
        [HttpGet]
        public async Task<ActionResult> OrderConfirmed1(int id, string inv) {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> OrderConfirmed(long id, string inv)
        {
            string siteBaseURL = _options.baseUrl;
            ViewBag.RedirectionURL = siteBaseURL + "cart/OrderPaymentStatus";
            List<OrderDisplayDTO> ordersList = null;
            OrderDisplayDTO currentOrder = null;
            string api_url = _options.apiurl;
            string upload_api = _options.UploadLocation;
            string folder_name = UploadLocations.Products;

            ViewBag.UrlPath = api_url + upload_api + folder_name;

            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    OrderManagementDTO orderObj = new OrderManagementDTO();
                    orderObj.BranchId = 0;
                    orderObj.Type = 1;
                    orderObj.StatusId = 1;
                    orderObj.OrderId = id;
                    orderObj.InvoiceNo = inv;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/VendorOrderManagementAPI/NewGetOrderDetails", orderObj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        //var Data = response.SelectToken("Data");
                        var Data = (JArray)response["Data"];
                        if (Status)
                        {
                            var top1 = JsonConvert.DeserializeObject<List<OrderDisplayDTO>>(Data.ToString());
                            currentOrder = top1.FirstOrDefault();
                            if (currentOrder.OrderType == 3 && currentOrder.OrderStatus == 2 && currentOrder.awbNo.Length != 12)
                            {

                                StringBuilder sb = new StringBuilder();
                                string data = "";
                                int itemCount = 0;
                                decimal wholeweight = 0;
                                decimal weightKG = 0;
                                foreach (var item in currentOrder.Items)
                                {
                                    string variants = "";
                                    foreach (var varts in item.Variants)
                                    {
                                        variants += varts.OptionValueEn + "/";
                                    }
                                    if (!string.IsNullOrEmpty(variants))
                                        variants = variants.Remove(variants.Length - 1, 1);

                                    sb.Append(item.NameEn + "(" + variants + "), Qty:" + item.Qty + ", ");
                                    itemCount = itemCount + item.Qty;
                                    wholeweight = wholeweight + (item.Weight * item.Qty);
                                }
                                weightKG = wholeweight / 1000;   // weight in grams
                                                                 //if (weightKG > 15)
                                                                 //{

                                //}

                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    sb.Length--;
                                    sb.Length--;
                                }
                                if (sb.Length > 250)
                                    sb.ToString().Substring(0, 249);


                                string passKey = System.Configuration.ConfigurationManager.AppSettings["SMSAPassKey"];
                                SMSADTO smsaObj = new SMSADTO()
                                {
                                    passKey = passKey,
                                    refNo = currentOrder.InvoiceNo,
                                    sentDate = currentOrder.OrderDate.ToString(),
                                    idNo = currentOrder.InvoiceNo,

                                    cName = currentOrder.UserDetails.Select(x => x.FirstName).First(),
                                    Cntry = currentOrder.UserDetails.Select(x => x.CountryName).First(),
                                    cCity = currentOrder.UserDetails.Select(x => x.CityName).First(),
                                    cZip = currentOrder.UserDetails.Select(x => x.ZipCode).First(),
                                    cPOBox = "",
                                    cMobile = currentOrder.UserDetails.Select(x => x.MobileNo).First(),
                                    cTel1 = currentOrder.UserDetails.Select(x => x.Mobile2).First(),
                                    cTel2 = "",
                                    cAddr1 = currentOrder.UserDetails.Select(x => x.Address1).First(),
                                    cAddr2 = currentOrder.UserDetails.Select(x => x.Address2).First(),

                                    shipType = "DLV",
                                    PCs = itemCount,
                                    cEmail = currentOrder.UserDetails.Select(x => x.EmailId).First(),
                                    codAmt = currentOrder.PaymentMode == "COD" ? currentOrder.GrandTotal.ToString() : "0.00",
                                    weight = weightKG.ToString("N2"),
                                    itemDesc = sb.ToString(),
                                    prefDelvDate = currentOrder.ExpectingDelivery,

                                    carrValue = "",
                                    carrCurr = "",
                                    custCurr = "",
                                    custVal = "",
                                    insrCurr = "",
                                    insrAmt = "",

                                    sName = currentOrder.ShipperAddress.Select(x => x.FirstName).First(),
                                    sContact = currentOrder.ShipperAddress.Select(x => x.MobileNo).First(),
                                    sAddr1 = currentOrder.ShipperAddress.Select(x => x.Address1).First(),
                                    sAddr2 = "",
                                    sCity = currentOrder.ShipperAddress.Select(x => x.CityName).First(),
                                    sPhone = currentOrder.ShipperAddress.Select(x => x.MobileNo).First(),
                                    sCntry = currentOrder.ShipperAddress.Select(x => x.CountryName).First(),


                                    gpsPoints = "",
                                    awbNo = "",
                                    reason = ""
                                };
                                HttpResponseMessage responseMessage2 = await client.PostAsJsonAsync("api/SMSAAPI/AddShipment", smsaObj);
                                if (responseMessage2.IsSuccessStatusCode)
                                {
                                    var responseData2 = responseMessage2.Content.ReadAsStringAsync().Result;
                                    var response2 = JObject.Parse(responseData2);
                                    bool Status2 = Convert.ToBoolean(response2.SelectToken("Status"));
                                    var Message2 = Convert.ToString(response2.SelectToken("Message"));
                                    var awbNo = Convert.ToString(response2.SelectToken("Data"));
                                    if (awbNo != null && awbNo != "" && awbNo.Length == 12)
                                    {
                                        currentOrder.awbNo = awbNo;
                                    }


                                }
                            }

                        }
                    }
                }
                catch (Exception Ex)
                {

                }
            }
            ViewData["CurrentOrder"] = currentOrder;
            return View(currentOrder);
        }
        public async Task<ActionResult> OrderPayment(long id, string inv)
        {
            TempData["OrderId"] = id;
            TempData["InvoiceNo"] = inv;
            string siteBaseURL = _options.baseUrl;
            ViewBag.RedirectionURL = siteBaseURL + "cart/OrderPaymentStatus";
            List<OrderDisplayDTO> ordersList = null;
            OrderDisplayDTO currentOrder = null;
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    OrderManagementDTO orderObj = new OrderManagementDTO();
                    orderObj.BranchId = 0;
                    orderObj.Type = 1;
                    orderObj.StatusId = 1;
                    orderObj.OrderId = id;
                    orderObj.InvoiceNo = inv;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/VendorOrderManagementAPI/NewGetOrderDetails", orderObj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        //var Data = response.SelectToken("Data");
                        var Data = (JArray)response["Data"];
                        if (Status)
                        {
                            var top1 = JsonConvert.DeserializeObject<List<OrderDisplayDTO>>(Data.ToString());
                            currentOrder = top1.FirstOrDefault();

                            ViewData["InvoiceNo"] = currentOrder.InvoiceNo;
                            ViewData["OrderAmount"] = currentOrder.GrandTotal.ToString("N2").Replace(",", "");
                            HyperpayCheckoutRequestDTO PayObj = new HyperpayCheckoutRequestDTO();
                            PayObj.userId = currentOrder.UserId.ToString();
                            PayObj.merchantTransactionId = currentOrder.InvoiceNo;
                            PayObj.isCardRegistration = false;
                            PayObj.amount = currentOrder.GrandTotal.ToString("N2");
                            PayObj.customerEmail = currentOrder.UserDetails.FirstOrDefault().EmailId;
                            PayObj.customerName = currentOrder.UserDetails.FirstOrDefault().FirstName;
                            PayObj.customerPhone = currentOrder.UserDetails.FirstOrDefault().MobileNo;
                            PayObj.paymentBy = 1;
                            PayObj.shopperResultUrl = "~/vendor/Branches/PaymentStatus";
                            if (AppUtils.BillingAddr != null)
                            {
                                BillingAddressDTO billAddr = (BillingAddressDTO)AppUtils.BillingAddr;
                                PayObj.Street1 = billAddr.street;
                                PayObj.City = billAddr.city;
                                PayObj.State = billAddr.region;
                                PayObj.PostCode = billAddr.postalcode;
                                PayObj.Country = "SA"; // billAddr.country;
                            }

                            if (currentOrder.PaymentMode.ToLower() == "mada")
                            {
                                PayObj.isMadaCard = true;
                                PayObj.isVisaMaster = false;
                                PayObj.isApplePay = false;
                            }
                            else if (currentOrder.PaymentMode.ToLower() == "cc")
                            {
                                PayObj.isMadaCard = false;
                                PayObj.isVisaMaster = true;
                                PayObj.isApplePay = false;
                            }
                            else if (currentOrder.PaymentMode.ToLower() == "CC")
                            {
                                PayObj.isMadaCard = false;
                                PayObj.isVisaMaster = false;
                                PayObj.isApplePay = true;
                            }


                            HttpResponseMessage payResponseMessage = await client.PostAsJsonAsync("api/HyperpayAPI/GetNewCheckoutId", PayObj);
                            if (payResponseMessage.IsSuccessStatusCode)
                            {
                                var payResponseData = payResponseMessage.Content.ReadAsStringAsync().Result;
                                var payResponse = JObject.Parse(payResponseData);
                                bool Status2 = Convert.ToBoolean(payResponse.SelectToken("Status"));
                                var Message2 = Convert.ToString(payResponse.SelectToken("Message"));
                                //var Data = response.SelectToken("Data");
                                var Data2 = (JArray)payResponse["Data"];
                                if (Status2)
                                {
                                    if (currentOrder != null)
                                    {
                                        currentOrder.CheckoutId = Data2[0].SelectToken("id").ToString();
                                        AppUtils.Trans_Id = currentOrder.CheckoutId;
                                        PayObj.resourcePath = Data2[0]["id"].ToString();
                                    }
                                }
                                else
                                {
                                    ViewData["Error2"] = payResponseData;
                                    ViewData["Error"] = Message2;
                                    return View("Error");
                                }
                            }
                            else
                            {
                                ViewData["Error2"] = "CheckOut Id Failed";
                                ViewData["Error"] = "CheckoutId Failed";

                                var payResponseData = payResponseMessage.Content.ReadAsStringAsync().Result;
                                var payResponse = JObject.Parse(payResponseData);
                                bool Status2 = Convert.ToBoolean(payResponse.SelectToken("Status"));
                                var Message2 = Convert.ToString(payResponse.SelectToken("Message"));
                                //var Data = response.SelectToken("Data");
                                var Data2 = (JArray)payResponse["Data"];
                                ViewData["Error2"] = payResponseData;
                                ViewData["Error"] = Message2;
                                return View("Error");
                            }
                            AppUtils.PayObj = PayObj;
                            ViewBag.PayMethod = currentOrder.PaymentMode;

                        }
                        else
                        {
                            ViewData["Error2"] = "OrderDetails Failed";
                            ViewData["Error"] = "OrderDetails Failed";
                            return View("Error");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewData["Error2"] = "Catch Exception";
                    ViewData["Error"] = "Catch Exception";
                    ErrorLogDTO err = new ErrorLogDTO();
                    AssignValuesToDTO.AssingDToValues(err, ex, "CartController/OrderPayment", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    return RedirectToAction("Error");
                }
                //return View(ordersList);
            }
            TempData.Keep();
            return View(currentOrder);
        }
        public async Task<ActionResult> OrderPaymentStatus()
        {
            string CheckOutId = string.Empty;
            //if (Session != null)
            {
                HyperpayCheckoutRequestDTO PayObj;
                if (AppUtils.PayObj != null)
                {
                    PayObj = AppUtils.PayObj;
                    if (AppUtils.Trans_Id != null)
                        CheckOutId = AppUtils.Trans_Id.ToString();
                    ViewBag.BaseURL = baseUrl;
                    ViewBag.CheckOutID = CheckOutId;
                    CheckOutId = AppUtils.Trans_Id.ToString();
                    TempData.Keep();
                    Dictionary<string, dynamic> responseData1;
                    using (HttpClient client = new HttpClient())
                    {
                        _commonHeader.setHeaders(client);
                        try
                        {
                            SessionDTO __session = AppUtils.UserLogin;
                            HyperpayCheckoutRequestDTO obj = new HyperpayCheckoutRequestDTO();
                            obj.resourcePath = CheckOutId;
                            obj.userId = __session.UserId.ToString();

                            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/HyperpayAPI/GetPaymentStatus", PayObj);
                            if (responseMessage.IsSuccessStatusCode)
                            {
                                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                                var response = JObject.Parse(responseData);
                                bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                                var Message = Convert.ToString(response.SelectToken("Message"));
                                var Data = response.SelectToken("Data");
                                responseData1 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Data[0].ToString());
                                if (responseData1["customPayment"] == "OK")
                                {
                                    //Success
                                    AppUtils.transactionId = responseData1["id"];
                                    ViewBag.PaymentSuccess = "Payment done Successfully";
                                    ViewBag.PaymentFailure = null;

                                    // Update Transaction successful informataion to DB against the order along with OrderId, InvoiceNo and TransactionId of payment
                                    string InvoiceNo = responseData1["merchantTransactionId"];
                                    TempData["PaymentStatus"] = "1";


                                    // No need to call update payment status

                                    #region Update Orders table with payment transaction id
                                    /*
                                    HyperPayLogsDTO HPayObj = new HyperPayLogsDTO();
                                    HPayObj.OrderId = Convert.ToInt64(TempData["OrderId"]);
                                    HPayObj.InvoiceNo = TempData["InvoiceNo"].ToString();
                                    HPayObj.TransactionId = responseData1["id"];

                                    HttpResponseMessage payResponseMessage = await client.PostAsJsonAsync("api/HyperpayAPI/UpdatePaymentStatus", HPayObj);
                                    if (payResponseMessage.IsSuccessStatusCode)
                                    {
                                        var payResponseData = payResponseMessage.Content.ReadAsStringAsync().Result;
                                        var payResponse = JObject.Parse(payResponseData);
                                        bool Status2 = Convert.ToBoolean(payResponse.SelectToken("Status"));
                                        var Message2 = Convert.ToString(payResponse.SelectToken("Message"));
                                        //var Data = response.SelectToken("Data");
                                        var Data2 = (JArray)payResponse["Data"];
                                        if (Status)
                                        {
                                            //if (currentOrder != null)
                                            //{
                                            //    currentOrder.CheckoutId = Data[0].SelectToken("id").ToString();
                                            //    Session["Trans_Id"] = currentOrder.CheckoutId;
                                            //}
                                        }
                                        else
                                        {
                                            return View("Error");
                                        }
                                    }

                                    */
                                    #endregion
                                    TempData.Keep();
                                    //return RedirectToAction("OrderConfirmed", "cart", new { id = TempData["OrderId"], inv = TempData["InvoiceNo"] });
                                    string url = "OrderConfirmed?id=" + TempData["OrderId"].ToString() + "&inv=" + TempData["InvoiceNo"].ToString();

                                    return Redirect(url);
                                }
                                else
                                {
                                    //Fail
                                    ViewBag.PaymentFailure = "Failure in Payment";//responseData1["result"]["Description"];
                                    ViewBag.PaymentSuccess = null;
                                    TempData["PaymentStatus"] = "0";
                                    //RedirectToAction("OrderPayment", "cart", new { id = TempData["OrderId"] , inv = TempData["InvoiceNo"] });
                                    string url = "OrderPayment?id=" + TempData["OrderId"].ToString() + "&inv=" + TempData["InvoiceNo"].ToString();
                                    return Redirect(url);
                                }
                            }
                            else
                            {

                            }

                        }
                        catch (Exception ex)
                        {
                            ErrorLogDTO err = new ErrorLogDTO();
                            //AssignValuesToDTO.AssingDToValues(err, ex, "AdvertisementController/Index", "India Standard Time.");
                            _errorHandler.WriteError(err, ex.Message);
                            return RedirectToAction("Error");
                        }
                    }
                }
                else
                {

                }
            }
            //else
            //{

            //}
            return View();
        }
        public async Task<ActionResult> ManageAddress()
        {
            CustomerManageAddressDTO user = new CustomerManageAddressDTO();
            if (AppUtils.UserLogin != null)
            {
                SessionDTO objs = AppUtils.UserLogin;
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

                            CountryDTO obj = new CountryDTO();
                            obj.Id = 0;
                            List<CountryDTO> getCountryList = await getListOfCountry(obj);
                            if (getCountryList != null)
                            {
                                user.CountryList = getCountryList;
                            }

                            CitysDTO objCity = new CitysDTO();
                            obj.Id = 0;
                            List<CitysDTO> getCityList = await getListOfCity(objCity);
                            if (getCityList != null)
                            {
                                user.CityList = getCityList;
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
            }
            else
            {
                return RedirectToAction("Index");
            }
            var result = await _viewRenderService.RenderToStringAsync("Cart/PVManageAddress", user);
            return Json(new { result = result.ToString() });
            //return PartialView("PVManageAddress", user);
        }
        [HttpPost]
        public async Task<ActionResult> ManageAddress(string data)
        {
            CustomerManageAddressDTO user = new CustomerManageAddressDTO();

            if (AppUtils.UserLogin != null)
            {
                SessionDTO objs = AppUtils.UserLogin;


                using (HttpClient client = new HttpClient())
                {
                    _commonHeader.setHeaders(client);
                    try
                    {
                        user = JsonConvert.DeserializeObject<CustomerManageAddressDTO>(data.ToString());
                        user.FlagId = 1;
                        user.UserId = Convert.ToInt64(objs.UserId);
                        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CustomerAPI/NewAddUpdateCustomerAddress", user);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                            var response = JObject.Parse(responseData);
                            bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                            var Message = Convert.ToString(response.SelectToken("Message"));
                            var Data = response.SelectToken("Data");
                            if (Data != null)
                            {
                                user.ListOfCustomerAddress = JsonConvert.DeserializeObject<List<CustomerManageAddressDTO>>(Data.ToString());
                                user.Id = user.ListOfCustomerAddress.Max(x => x.Id);
                                user.Status = Status;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ////ErrorLogDTO err = new ErrorLogDTO();
                        ////  AssignValuesToDTO.AssingDToValues(err, ex, "Vendor/BranchesController/SaveBranchDescription", "India Standard Time.");
                        ////_errorHandler.WriteError(err, ex.Message);
                    }
                }

            }
            return Json(user);
            //return RedirectToAction("ManageAddress");
        }
        //public JsonResult ShippingExpectedDate(string CityName)
        public PartialViewResult ShippingExpectedDate(long CityId)
        {

            //var date = Utils.Common.KSA_DateTime();
            //var day = date.DayOfWeek.ToString();

            //if (CityName == "Jeddah" || CityName == "Riyadh" || CityName == "Dammam")
            //{
            //    if (day == "Thursday")
            //    {
            //        date = date.AddDays(2);
            //    }
            //    else
            //    {
            //        date = date.AddDays(1);
            //    }
            //}
            //else
            //{
            //    if (day == "Wednesday")
            //    {
            //        date = date.AddDays(3);
            //    }
            //    else
            //    {
            //       date = date.AddDays(2);
            //    }
            //}
            PaymentOptionDTO obj = new PaymentOptionDTO();
            bool success = false;
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    SessionDTO _login = AppUtils.UserLogin;
                    obj.CreatedBy = _login.UserId;
                    obj.CityId = CityId;
                    obj.OrderType = 3;
                    obj.Type = 1;

                    HttpResponseMessage responseMessage = client.PostAsJsonAsync("api/ProductsInfoAPI/NewGetOrderBranchPayment", obj).Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var json = JObject.Parse(responseData);
                        success = Convert.ToBoolean(json.SelectToken("Status"));
                        if (success)
                        {
                            var data = json.SelectToken("Data");
                            var paymentDate = data.SelectToken("PaymentData").ToString();
                            if (paymentDate != "[]")
                            {
                                obj = JsonConvert.DeserializeObject<List<PaymentOptionDTO>>(paymentDate).FirstOrDefault();
                                obj.Type = 1;
                                obj.IsPayment = true;
                                obj.IsApplePayAllowed = false;
                            }
                            obj.CityName = Convert.ToString(data.SelectToken("CityName"));
                            obj.IsShipment = Convert.ToBoolean(data.SelectToken("IsShipment"));
                            obj.ExpectedDate = Convert.ToString(data.SelectToken("ExpectedDate"));
                            obj.Status = true;

                            //obj.IsShipment = Convert.ToBoolean(data.SelectToken("IsShipment"));
                            //obj.CityName  = Convert.ToString(data.SelectToken("CityName"));
                            //obj.ExpectedDate = Convert.ToString(data.SelectToken("ExpectedDate"));
                            //obj.Status = true;
                            //obj.IsCashAllowed = true;
                            //obj.IsCreditCardAllowed = true;
                            //obj.IsCcPayLater = true;
                            //obj.IsMadaCardAllowed = true;
                            //obj.IsApplePayAllowed = false;
                            //obj.IsCcPayNow = true;
                            //obj.IsPayment = true;
                        }
                        else
                        {
                            obj.IsShipment = false;
                            obj.Status = false;
                            obj.IsPayment = true;
                        }
                        return PartialView("_PVOrderPaymentOptions", obj);
                        //return Json(obj, JsonRequestBehavior.AllowGet);
                    }
                    //return Json(obj, JsonRequestBehavior.AllowGet);
                    return PartialView("_PVOrderPaymentOptions", obj);
                }
                catch (Exception ex)
                {
                    //return Json(obj, JsonRequestBehavior.AllowGet);
                    return PartialView("_PVOrderPaymentOptions", obj);
                }
            }
            //return Json(date.ToShortDateString(),JsonRequestBehavior.AllowGet);
            return PartialView("_PVOrderPaymentOptions", obj);
        }
        public ActionResult UpdateRemakrs(string remarks)
        {

            long CouponId = -1;
            bool success = false;
            string successmessage = "";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    AddCouponToCartDTO a = new AddCouponToCartDTO();
                    //CartDTO a = new CartDTO();
                    SessionDTO _login = AppUtils.UserLogin;
                    a.UserId = _login.UserId;
                    a.CouponId = (int)CouponId;
                    a.Remarks = remarks;

                    HttpResponseMessage responseMessage = client.PostAsJsonAsync("api/OrdersAPI/NewAddCouponToCart", a).Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        //var user = JsonConvert.DeserializeObject<CartDTO>(responseData);
                        var json = JObject.Parse(responseData);
                        success = Convert.ToBoolean(json.SelectToken("Status"));
                        successmessage = Convert.ToString(json.SelectToken("Message"));

                    }
                    return RedirectToAction("Checkout");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Checkout");
                }
            }
        }
        public async Task<JsonResult> ValidatePaymentMethodsAsync(long CityId, int OrderType, int type = 1)
        {
            PaymentOptionDTO obj = new PaymentOptionDTO();
            bool success = false;
            string successmessage = "";
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    SessionDTO _login = AppUtils.UserLogin;
                    obj.CreatedBy = _login.UserId;
                    obj.CityId = CityId;
                    obj.OrderType = OrderType;


                    HttpResponseMessage responseMessage = client.PostAsJsonAsync("api/ProductsInfoAPI/NewGetOrderBranchPayment", obj).Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        //var user = JsonConvert.DeserializeObject<CartDTO>(responseData);
                        var json = JObject.Parse(responseData);
                        success = Convert.ToBoolean(json.SelectToken("Status"));
                        if (success)
                        {
                            var data = json.SelectToken("Data");
                            var paymentDate = data.SelectToken("PaymentData").ToString();
                            if (paymentDate != "[]")
                            {
                                obj = JsonConvert.DeserializeObject<List<PaymentOptionDTO>>(paymentDate).FirstOrDefault();
                                obj.IsPayment = true;
                                obj.IsApplePayAllowed = false;

                            }
                            obj.CityName = Convert.ToString(data.SelectToken("CityName"));
                            obj.IsShipment = Convert.ToBoolean(data.SelectToken("IsShipment"));
                        }
                        else
                        {
                            obj.IsPayment = false;
                            obj.IsApplePayAllowed = false;
                        }
                        obj.BranchId = _login.BranchId;
                        obj.Type = type;
                        var result = await _viewRenderService.RenderToStringAsync("Cart/_PVOrderPaymentOptions", obj);
                        return Json(new {result = result.ToString() });
                        //return PartialView("_PVOrderPaymentOptions", obj);
                        //return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    obj.Type = type;
                    var result1 = await _viewRenderService.RenderToStringAsync("Cart/_PVOrderPaymentOptions", obj);
                    return Json(new { result = result1.ToString() });
                }
                catch (Exception ex)
                {
                    obj.Type = type;
                    var result2 = await _viewRenderService.RenderToStringAsync("Cart/_PVOrderPaymentOptions", obj);
                    return Json(new { result = result2.ToString() });
                }
            }
        }

        public async Task<JsonResult> PVOrderPayment(long id, string inv, string PaymentMode, string PaymentType)
        {
            //TempData["OrderId"] = id;
            //TempData["InvoiceNo"] = inv;
            ViewBag.OrderId = id;
            ViewBag.InvoiceNo = inv;
            string siteBaseURL = _options.baseUrl; //baseUrl; //ConfigurationManager.AppSettings["baseUrl"].ToString();
            ViewBag.RedirectionURL = siteBaseURL + "cart/OrderPaymentStatus";
            List<OrderDisplayDTO> ordersList = null;
            OrderDisplayDTO currentOrder = null;
            using (HttpClient client = new HttpClient())
            {
                _commonHeader.setHeaders(client);
                try
                {
                    OrderManagementDTO orderObj = new OrderManagementDTO();
                    orderObj.BranchId = 0;
                    orderObj.Type = 1;
                    orderObj.StatusId = 1;
                    orderObj.OrderId = id;
                    orderObj.InvoiceNo = inv;

                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/VendorOrderManagementAPI/NewGetOrderDetails", orderObj);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        var response = JObject.Parse(responseData);
                        bool Status = Convert.ToBoolean(response.SelectToken("Status"));
                        var Message = Convert.ToString(response.SelectToken("Message"));
                        //var Data = response.SelectToken("Data");
                        var Data = (JArray)response["Data"];
                        if (Status)
                        {
                            var top1 = JsonConvert.DeserializeObject<List<OrderDisplayDTO>>(Data.ToString());
                            currentOrder = top1.FirstOrDefault();

                            ViewBag.InvoiceNo = currentOrder.InvoiceNo;
                            ViewBag.OrderAmount = currentOrder.GrandTotal.ToString("N2");
                            HyperpayCheckoutRequestDTO PayObj = new HyperpayCheckoutRequestDTO();
                            PayObj.userId = currentOrder.UserId.ToString();
                            PayObj.merchantTransactionId = currentOrder.InvoiceNo;
                            PayObj.isCardRegistration = false;
                            PayObj.amount = currentOrder.GrandTotal.ToString("N2").Replace(",", "");
                            PayObj.customerEmail = currentOrder.UserDetails.FirstOrDefault().EmailId;
                            PayObj.customerName = currentOrder.UserDetails.FirstOrDefault().FirstName;
                            PayObj.customerPhone = currentOrder.UserDetails.FirstOrDefault().MobileNo;
                            PayObj.paymentBy = 1;
                            PayObj.shopperResultUrl = "~/vendor/Branches/PaymentStatus";
                            if (AppUtils.BillingAddr != null)
                            {
                                BillingAddressDTO billAddr = (BillingAddressDTO)AppUtils.BillingAddr;
                                PayObj.Street1 = billAddr.street;
                                PayObj.City = billAddr.city;
                                PayObj.State = billAddr.region;
                                PayObj.PostCode = billAddr.postalcode;
                                PayObj.Country = "SA"; // billAddr.country;
                            }


                            if (currentOrder.PaymentMode != PaymentMode || currentOrder.PaymentType != PaymentType)
                            {
                                // update payment methods to Db
                                OrderDisplayDTO obj = new OrderDisplayDTO();
                                obj.PaymentType = PaymentType;
                                obj.PaymentMode = PaymentMode;
                                obj.OrderId = id;

                                HttpResponseMessage respUpdatePaymentmethod = await client.PostAsJsonAsync("api/OrdersAPI/NewUpdateOrderPaymentMethod", obj);
                                if (respUpdatePaymentmethod.IsSuccessStatusCode)
                                {
                                    var payResponseData = respUpdatePaymentmethod.Content.ReadAsStringAsync().Result;
                                    var payResponse = JObject.Parse(payResponseData);
                                    bool Status2 = Convert.ToBoolean(payResponse.SelectToken("Status"));
                                }

                                currentOrder.PaymentMode = PaymentMode;
                                currentOrder.PaymentType = PaymentType;
                            }

                            if (currentOrder.PaymentMode.ToLower() == "mada")
                            {
                                PayObj.isMadaCard = true;
                                PayObj.isVisaMaster = false;
                                PayObj.isApplePay = false;
                            }
                            else if (currentOrder.PaymentMode.ToLower() == "cc")
                            {
                                PayObj.isMadaCard = false;
                                PayObj.isVisaMaster = true;
                                PayObj.isApplePay = false;
                            }
                            else if (currentOrder.PaymentMode.ToLower() == "CC")
                            {
                                PayObj.isMadaCard = false;
                                PayObj.isVisaMaster = false;
                                PayObj.isApplePay = true;
                            }

                            //Session["PayObj"] = PayObj;
                            HttpResponseMessage payResponseMessage = await client.PostAsJsonAsync("api/HyperpayAPI/GetNewCheckoutId", PayObj);
                            if (payResponseMessage.IsSuccessStatusCode)
                            {
                                var payResponseData = payResponseMessage.Content.ReadAsStringAsync().Result;
                                var payResponse = JObject.Parse(payResponseData);
                                bool Status2 = Convert.ToBoolean(payResponse.SelectToken("Status"));
                                var Message2 = Convert.ToString(payResponse.SelectToken("Message"));
                                //var Data = response.SelectToken("Data");
                                var Data2 = (JArray)payResponse["Data"];
                                if (Status2)
                                {
                                    if (currentOrder != null)
                                    {
                                        currentOrder.CheckoutId = Data2[0].SelectToken("id").ToString();
                                        AppUtils.Trans_Id = currentOrder.CheckoutId;
                                        PayObj.resourcePath = Data2[0]["id"].ToString();
                                    }
                                }
                                else
                                {
                                    //ViewData["Error2"] = payResponseData;
                                    //ViewData["Error"] = Message2; 
                                    ViewBag.Error2 = payResponseData;
                                    ViewBag.Error = Message2; 
                                }
                            }
                            else
                            {
                                //ViewData["Error2"] = "CheckOut Id Failed";
                                //ViewData["Error"] = "CheckoutId Failed";
                                ViewBag.Error2 = "CheckOut Id Failed";
                                ViewBag.Error = "CheckoutId Failed";

                                var payResponseData = payResponseMessage.Content.ReadAsStringAsync().Result;
                                var payResponse = JObject.Parse(payResponseData);
                                bool Status2 = Convert.ToBoolean(payResponse.SelectToken("Status"));
                                var Message2 = Convert.ToString(payResponse.SelectToken("Message"));
                                //var Data = response.SelectToken("Data");
                                var Data2 = (JArray)payResponse["Data"];
                                //ViewData["Error2"] = payResponseData;
                                //ViewData["Error"] = Message2;
                                ViewBag.Error2 = payResponseData;
                                ViewBag.Error = Message2;
                                //return View("Error");
                            }
                            AppUtils.PayObj = PayObj;
                            ViewBag.PayMethod = currentOrder.PaymentMode;
                            ViewBag.PaymentStatus = TempData["PaymentStatus"];
                        }
                        else
                        {
                            //ViewData["Error2"] = "OrderDetails Failed";
                            //ViewData["Error"] = "OrderDetails Failed";
                            ViewBag.Error2 = "OrderDetails Failed";
                            ViewBag.Error = "OrderDetails Failed";
                            //return View("Error");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ViewData["Error2"] = "Catch Exception";
                    //ViewData["Error"] = "Catch Exception";
                    ViewBag.Error2 = "Catch Exception";
                    ViewBag.Error = "Catch Exception";
                    ErrorLogDTO err = new ErrorLogDTO();
                    AssignValuesToDTO.AssingDToValues(err, ex, "CartController/OrderPayment", "India Standard Time.");
                    _errorHandler.WriteError(err, ex.Message);
                    //return RedirectToAction("Error");
                }
                //return View(ordersList);
            }
            //TempData.Keep();
            try {
                var result = await _viewRenderService.RenderToStringAsync("Cart/PVOrderPayment", currentOrder);
                var a = TempData["PaymentStatus"];
            }
            catch (Exception ex)
            { 
            
            }
            var result2 = await _viewRenderService.RenderToStringAsync("Cart/PVOrderPayment", currentOrder);
            return Json(new { result = result2.ToString() });
            //return PartialView(currentOrder);
        }
    }
}
