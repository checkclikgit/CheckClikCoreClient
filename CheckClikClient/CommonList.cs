using CheckClikClient.Handlers;
using Customer.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CheckClikClient
{
    public class CommonList
    {
        string currentUrl = "";
        string baseUrl = "";
        string Profileurl = ""; 

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ErrorHandler _errorHandler;
        private readonly AppSettingsKeysInfo _options;
        private CommonHeader _commonHeader;//= new CommonHeader();

        public CommonList(IHttpContextAccessor httpContextAccessor, ErrorHandler errorHandler, IOptions<AppSettingsKeysInfo> options, CommonHeader commonHeader)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options.Value; 
            var baseURL = _options.baseUrl;
            currentUrl = _httpContextAccessor.HttpContext.Request.PathBase.ToString();
            currentUrl = currentUrl.Replace(baseURL, baseURL + "ar/");
            Profileurl = _options.profileUrl;
             
            _errorHandler = errorHandler;
            _commonHeader = commonHeader;
        }


        //public static async Task<List<CountryDTO>> getListOfCountry(CountryDTO obj)
        //{
        //    List<CountryDTO> CountryList = new List<CountryDTO>();
        //    using (HttpClient client = new HttpClient())
        //    { 
        //_commonHeader.setHeaders(client);
        //        try
        //        {
        //            // obj.Id = 0;
        //            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CountryAPI/NewGetCountryDetails", obj);
        //            if (responseMessage.IsSuccessStatusCode)
        //            {
        //                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
        //                var Data = JsonConvert.DeserializeObject<List<CountryDTO>>(responseData);
        //                CountryList = Data; 
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    return CountryList;
        //}

        //public static async Task<List<CitysDTO>> getListOfCity(CitysDTO obj)
        //{
        //    List<CitysDTO> CitysList = new List<CitysDTO>();
        //    using (HttpClient client = new HttpClient())
        //    {
        //        //CommonHeader.setHeaders(client);
        //        try
        //        {
        //            // obj.Id = 0;
        //            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/CityAPI/NewGetCityDetails", obj);
        //            if (responseMessage.IsSuccessStatusCode)
        //            {
        //                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
        //                var Data = JsonConvert.DeserializeObject<List<CitysDTO>>(responseData);
        //                CitysList = Data;
                         
        //            }
        //        }
        //        catch (Exception ex)
        //        { 
        //        }
        //    }
        //    return CitysList;
        //}
    }
}
