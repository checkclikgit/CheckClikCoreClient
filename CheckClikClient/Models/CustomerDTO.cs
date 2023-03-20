using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class CustomerDTO
    {
        public long MapId { get; set; }

        public int BranchId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string datasetxml { get; set; }
        public int StoreId { get; set; }
        public long UserId { get; set; }
        public IEnumerable<CustomerDTO> Images_dt { get; set; }

        public string base64 { get; set; }
        public string FileName { get; set; }
        public string FileUploadLocation { get; set; }
        public byte[] Bytes { get; set; }

        public int Id { get; set; }
        public long CreatedBy { get; set; }
        public string message { get; set; }

        public string return_url { get; set; }
        public string ImagesJson { get; set; }

        public string ServiceJson { get; set; }
        public string Branchjson { get; set; }
        public string Mapjson { get; set; }

        public int pagingNumber { get; set; }
        public int PageNumber { get; set; }
        public long TotalRecords { get; set; }
        public int PageSize { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ServiceName { get; set; }
        public int Publised { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public int ServiceProviderId { get; set; }
        public int ServiceType { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }


        public string OrderStatus { get; set; }
        public int PaymentType { get; set; }
        public int Ratings { get; set; }
        public int Distance { get; set; }
        public int Sort { get; set; }
        public string ContactNo { get; set; }
        public int Type { get; set; }
        public int UserType { get; set; }
        public string StoreName {get;set;}
        public string SearchText { get; set; }
        public bool IsWeb { get; set; }
        public int FlagId { get;  set; }
    }
}