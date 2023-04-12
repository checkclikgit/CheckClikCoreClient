using CheckClickClient;
using CheckClickClient.Models;
using CheckClikClient.Models;
using Customer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SearchLibrary.Models;
using SearchLibrary;

namespace CheckClickClient
{
    public static class AppUtils
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current => _httpContextAccessor.HttpContext;
        //public static VMPatientBasicInformation PatientUser
        //{
        //    get
        //    {
        //        var loggedInUserSessionString = Current.Session.GetString(SessionKeyConstants.LOGGEDIN_USER_KEY);
        //        VMPatientBasicInformation patientUser = new VMPatientBasicInformation();
        //        if (loggedInUserSessionString != null)
        //        {
        //            patientUser = JsonConvert.DeserializeObject<VMPatientBasicInformation>(loggedInUserSessionString);
        //        } 
        //        return patientUser;
        //    }
        //    set { Current.Session.SetString(SessionKeyConstants.LOGGEDIN_USER_KEY, JsonConvert.SerializeObject(value)); }
        //}
        public static SessionDTO UserLogin
        {
            get
            {
                var loggedInUserSessionString = Current.Session.GetString(SessionKeyConstants.LOGGEDIN_USER_KEY);
                SessionDTO loggedInUser = new SessionDTO();
                if (loggedInUserSessionString != null)
                {
                    loggedInUser = JsonConvert.DeserializeObject<SessionDTO>(loggedInUserSessionString);
                }
                return loggedInUser;
            }
            set { Current.Session.SetString(SessionKeyConstants.LOGGEDIN_USER_KEY, JsonConvert.SerializeObject(value)); }
        }
        public static List<AdminMenuCategoryDTO> MenuMainCategory
        {
            get
            {
                var menyMainCategoryString = Current.Session.GetString(SessionKeyConstants.MENU_MAIN_CATEGORY);
                List<AdminMenuCategoryDTO> mainCategoryDTO = new List<AdminMenuCategoryDTO>();
                if (menyMainCategoryString != null)
                {
                    mainCategoryDTO = JsonConvert.DeserializeObject<List<AdminMenuCategoryDTO>>(menyMainCategoryString);
                }
                return mainCategoryDTO;
            }
            set { Current.Session.SetString(SessionKeyConstants.MENU_MAIN_CATEGORY, JsonConvert.SerializeObject(value)); }
        }
        public static List<AdminMenuCategoryDTO> SubMainCategory
        {
            get
            {
                var menuSubMainCategoryString = Current.Session.GetString(SessionKeyConstants.SUB_MAIN_CATEGORY);
                List<AdminMenuCategoryDTO> subMainCategoryDTO = new List<AdminMenuCategoryDTO>();
                if (menuSubMainCategoryString != null)
                {
                    subMainCategoryDTO = JsonConvert.DeserializeObject<List<AdminMenuCategoryDTO>>(menuSubMainCategoryString);
                }
                return subMainCategoryDTO;
            }
            set { Current.Session.SetString(SessionKeyConstants.SUB_MAIN_CATEGORY, JsonConvert.SerializeObject(value)); }
        }
        public static List<CityDTO> CityList
        {
            get
            {
                var cityListString = Current.Session.GetString(SessionKeyConstants.CITY_LIST);
                List<CityDTO> cityDTO = new List<CityDTO>();
                if (cityListString != null)
                {
                    cityDTO = JsonConvert.DeserializeObject<List<CityDTO>>(cityListString);
                }
                return cityDTO;
            }
            set
            {
                Current.Session.SetString(SessionKeyConstants.CITY_LIST, JsonConvert.SerializeObject(value));
            }
        }
        public static List<DistrictDTO> DistrictList
        {
            get
            {
                var districtListString = Current.Session.GetString(SessionKeyConstants.DISTRICT_LIST);
                List<DistrictDTO> districtDTO = new List<DistrictDTO>();
                if (districtListString != null)
                {
                    districtDTO = JsonConvert.DeserializeObject<List<DistrictDTO>>(districtListString);
                }
                return districtDTO;
            }
            set { Current.Session.SetString(SessionKeyConstants.DISTRICT_LIST, JsonConvert.SerializeObject(value)); }
        }
        public static string SelectedCity
        {
            get
            {
                var selectedCityString = Current.Session.GetString(SessionKeyConstants.SELECTED_CITY);
                string sCity = "";
                if (selectedCityString != null)
                {
                    sCity = JsonConvert.DeserializeObject<string>(selectedCityString);
                }
                return sCity;
            }
            set { Current.Session.SetString(SessionKeyConstants.SELECTED_CITY, JsonConvert.SerializeObject(value)); }
        }
        public static string SelectedDistrict
        {
            get
            {
                var selectedDistrictString = Current.Session.GetString(SessionKeyConstants.SELECTED_DISTRICT);
                string sCity = "";
                if (selectedDistrictString != null)
                {
                    sCity = JsonConvert.DeserializeObject<string>(selectedDistrictString);
                }
                return sCity;
            }
            set { Current.Session.SetString(SessionKeyConstants.SELECTED_DISTRICT, JsonConvert.SerializeObject(value)); }
        }
        public static string SearchBranchId
        {
            get
            {
                var searchBrandString = Current.Session.GetString(SessionKeyConstants.SEARCH_BRAND_ID);
                string searchBrand = "";
                if (searchBrandString != null)
                {
                    searchBrand = JsonConvert.DeserializeObject<string>(searchBrandString);
                }
                return searchBrand;
            }
            set
            {
                try
                {

                    //Current.Session.SetString(SessionKeyConstants.SEARCH_BRAND_ID, value);
                    Current.Session.SetString(SessionKeyConstants.SEARCH_BRAND_ID, JsonConvert.SerializeObject(value));
                }

                catch(Exception e) 
                {
                    
                }
            }
        }
        public static string StoreChatingList
        {
            get
            {
                var chatDataString = Current.Session.GetString(SessionKeyConstants.STORE_CHATING_LIST);
                string chatData = "";
                if (chatDataString != null)
                {
                    chatData = JsonConvert.DeserializeObject<string>(chatDataString);
                }
                return chatData;
            }
            set { Current.Session.SetString(SessionKeyConstants.STORE_CHATING_LIST, JsonConvert.SerializeObject(value)); }
        }
        public static string SupportGroupId
        {
            get
            {
                var supportGroupString = Current.Session.GetString(SessionKeyConstants.SUPPORT_GROUP_ID);
                string supportGroup = "";
                if (supportGroupString != null)
                {
                    supportGroup = JsonConvert.DeserializeObject<string>(supportGroupString);
                }
                return supportGroup;
            }
            set { Current.Session.SetString(SessionKeyConstants.SUPPORT_GROUP_ID, JsonConvert.SerializeObject(value)); }
        }
        public static string UserId
        {
            get
            {
                var useridString = Current.Session.GetString(SessionKeyConstants.USER_ID);
                string userid = "";
                if (useridString != null)
                {
                    userid = JsonConvert.DeserializeObject<string>(useridString);
                }
                return userid;
            }
            set { Current.Session.SetString(SessionKeyConstants.USER_ID, JsonConvert.SerializeObject(value)); }
        }
        public static string UserLatLng
        {
            get
            {
                var userLatLngString = Current.Session.GetString(SessionKeyConstants.USER_ID);
                string userLatLng = "";
                if (userLatLngString != null)
                {
                    userLatLng = JsonConvert.DeserializeObject<string>(userLatLngString);
                }
                return userLatLng;
            }
            set { Current.Session.SetString(SessionKeyConstants.USER_ID, JsonConvert.SerializeObject(value)); }
        }
        public static IEnumerable<CartDTO> CartList
        {
            //List<DistrictDTO> districtDTO = new List<DistrictDTO>();
            //    if (districtListString != null)
            //    {
            //        districtDTO = JsonConvert.DeserializeObject<List<DistrictDTO>>(districtListString);
            //    }
            get
            {
                var cartListString = Current.Session.GetString(SessionKeyConstants.CART_LIST);
                List<CartDTO> cartList = new List<CartDTO>();
                if (cartListString != null)
                {
                    cartList = JsonConvert.DeserializeObject<List<CartDTO>>(cartListString);
                }
                return cartList;
            }
            set { Current.Session.SetString(SessionKeyConstants.CART_LIST, JsonConvert.SerializeObject(value)); }
        }

        public static BaseViewModel Branch
        {
            get
            {
                var branchString = Current.Session.GetString(SessionKeyConstants.Branch);
                BaseViewModel baseViewModel = new BaseViewModel();
                if (branchString != null)
                {
                    baseViewModel = JsonConvert.DeserializeObject<BaseViewModel>(branchString);
                }
                return baseViewModel;
            }
            set { Current.Session.SetString(SessionKeyConstants.Branch, JsonConvert.SerializeObject(value)); }
        }
        public static List<BranchDTO> BranchListByStoreId
        {
            get
            {
                var branchListString = Current.Session.GetString(SessionKeyConstants.Branch_List);
                List<BranchDTO> lstBranch = new List<BranchDTO>();
                if (branchListString != null)
                {
                    lstBranch = JsonConvert.DeserializeObject<List<BranchDTO>>(branchListString);
                }
                return lstBranch;
            }
            set { Current.Session.SetString(SessionKeyConstants.Branch_List, JsonConvert.SerializeObject(value)); }
        }
        public static string SearchString
        {
            get
            {
                var branchString = Current.Session.GetString(SessionKeyConstants.Search_String);
                string stringModel = "";
                if (branchString != null)
                {
                    stringModel = JsonConvert.DeserializeObject<string>(branchString);
                }
                return stringModel;
            }
            set { Current.Session.SetString(SessionKeyConstants.Search_String, JsonConvert.SerializeObject(value)); }
        }
        public static string PriceRange
        {
            get
            {
                var priceRranceString = Current.Session.GetString(SessionKeyConstants.Price_Range);
                string stringModel = "";
                if (priceRranceString != null)
                {
                    stringModel = JsonConvert.DeserializeObject<string>(priceRranceString);
                }
                return stringModel;
            }
            set { Current.Session.SetString(SessionKeyConstants.Price_Range, JsonConvert.SerializeObject(value)); }
        }
        public static int TotalSearchResults
        {
            get
            {
                var totalSearchCount = Current.Session.GetString(SessionKeyConstants.Total_Search_Results);
                int intModel = 0;
                if (totalSearchCount != null)
                {
                    intModel = JsonConvert.DeserializeObject<int>(totalSearchCount);
                }
                return intModel;
            }
            set { Current.Session.SetString(SessionKeyConstants.Total_Search_Results, JsonConvert.SerializeObject(value)); }
        } 
        public static string SearchModel
        {
            get
            {
                var totalSearchCount = Current.Session.GetString(SessionKeyConstants.Search_Model);
                string model = "";
                if (totalSearchCount != null)
                {
                    model = JsonConvert.DeserializeObject<string>(totalSearchCount);
                }
                return model;
            }
            set { Current.Session.SetString(SessionKeyConstants.Search_Model, JsonConvert.SerializeObject(value)); }
        } 
        public static QueryResponse<ProductSearchResult> SearchResult
        {
            get
            {
                var totalSearchCount = Current.Session.GetString(SessionKeyConstants.Search_Result);
                QueryResponse<ProductSearchResult> model = new QueryResponse<ProductSearchResult>();
                if (totalSearchCount != null)
                {
                    model = JsonConvert.DeserializeObject<QueryResponse<ProductSearchResult>>(totalSearchCount);
                }
                return model;
            }
            set { Current.Session.SetString(SessionKeyConstants.Search_Result, JsonConvert.SerializeObject(value)); }
        } 
        public static List<ProductDetailsDTO> ProductDetailsLst
        {
            get
            {
                var productDetailsListString = Current.Session.GetString(SessionKeyConstants.Product_Details_Lst);
                List<ProductDetailsDTO> lstProductDetails = new List<ProductDetailsDTO>();
                if (productDetailsListString != null)
                {
                    lstProductDetails = JsonConvert.DeserializeObject<List<ProductDetailsDTO>>(productDetailsListString);
                }
                return lstProductDetails;
            }
            set { Current.Session.SetString(SessionKeyConstants.Product_Details_Lst, JsonConvert.SerializeObject(value)); }
        }
        public static BillingAddressDTO BillingAddr
        {
            get
            {
                var billingAddressString = Current.Session.GetString(SessionKeyConstants.Billing_Address);
                BillingAddressDTO productDetails = new BillingAddressDTO();
                if (billingAddressString != null)
                {
                    productDetails = JsonConvert.DeserializeObject<BillingAddressDTO>(billingAddressString);
                }
                return productDetails;
            }
            set { Current.Session.SetString(SessionKeyConstants.Billing_Address, JsonConvert.SerializeObject(value)); }
        }
        public static HyperpayCheckoutRequestDTO PayObj
        {
            get
            {
                var HyperpayCheckoutRequestString = Current.Session.GetString(SessionKeyConstants.Pay_Obj);
                HyperpayCheckoutRequestDTO HyperpayCheckoutRequestDTO = new HyperpayCheckoutRequestDTO();
                if (HyperpayCheckoutRequestString != null)
                {
                    HyperpayCheckoutRequestDTO = JsonConvert.DeserializeObject<HyperpayCheckoutRequestDTO>(HyperpayCheckoutRequestString);
                }
                return HyperpayCheckoutRequestDTO;
            }
            set { Current.Session.SetString(SessionKeyConstants.Pay_Obj, JsonConvert.SerializeObject(value)); }
        }
        public static string Trans_Id
        {
            get
            {
                var Trans_IdString = Current.Session.GetString(SessionKeyConstants.Trans_Id);
                string Trans_IdDTO = "";
                if (Trans_IdString != null)
                {
                    Trans_IdDTO = JsonConvert.DeserializeObject<string>(Trans_IdString);
                }
                return Trans_IdDTO;
            }
            set { Current.Session.SetString(SessionKeyConstants.Trans_Id, JsonConvert.SerializeObject(value)); }
        }
        public static string transactionId
        {
            get
            {
                var transactionIdString = Current.Session.GetString(SessionKeyConstants.Transaction_Id);
                string TransIdDTO = "";
                if (transactionIdString != null)
                {
                    TransIdDTO = JsonConvert.DeserializeObject<string>(transactionIdString);
                }
                return TransIdDTO;
            }
            set { Current.Session.SetString(SessionKeyConstants.Trans_Id, JsonConvert.SerializeObject(value)); }
        }

    }
}
