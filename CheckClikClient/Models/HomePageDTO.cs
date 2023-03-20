using Customer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckClickClient.Models
{
    public class HomePageDTO
    {

        public List<HomePageSliderDTO> listProductOffer { get; set; }
        public List<HomePageSliderDTO> listServiceOffer { get; set; }
        public List<BannersDTO> listBanners { get; set; }
        public string sessionId { get; set; } 
        public string _authToken { get; set; }
        public string ApiURL { get; set; }
        public string Message { get; set; }
        public int IsBlocked { get; set; }
        public HomePageDTO()
        {
            //this.ApiURL = System.Configuration.ConfigurationManager.AppSettings["apiurl"].ToString();

        }
    }
}