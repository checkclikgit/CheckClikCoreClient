using Newtonsoft.Json.Linq;

namespace CheckClikClient.Models
{
    public class ProductsInfoDTO
    {
        public long OrderId { get; set; }

        public int WeekDayId { get; set; }

        public string PickupInServiceWeekdays { get; set; } = "";
        public string DeliveryHomeWeekdays { get; set; } = "";

        public long UserId { get; set; }
        public long ProductInventoryId { get; set; }
        //public string ControllerName { get; set; }
        //public string ActionName { get; set; }
        public JObject Data { get; set; }
        public int BranchId { get; set; }
        public int StoreId { get; set; }
        public string datasetxml { get; set; } = "";

        public string SkuId { get; set; } = "";
        public int LowStock { get; set; }

        public string message { get; set; } = "";

        public string ProductInfoJson { get; set; } = "";
        public string MatrixJson { get; set; } = "";
        public string InventoryJson { get; set; } = "";
        public string ImagesJson { get; set; } = "";

        public IEnumerable<ProductsInfoDTO> SkuIds_dt { get; set; }
        public IEnumerable<ProductsInfoDTO> Images_dt { get; set; }

        public string base64 { get; set; } = "";
        public string FileName { get; set; } = "";
        public string FileUploadLocation { get; set; } = "";
        public byte[] Bytes { get; set; }

        public int Id_datatable { get; set; }

        public FileAttribs _URLLocation { get; set; }

        public int Id { get; set; }

        public string return_url { get; set; } = "";
        public long CreatedBy { get; set; }

        public int pagingNumber { get; set; }
        public int PageNumber { get; set; }
        public long TotalRecords { get; set; }
        public int PageSize { get; set; }

        public string FromDate { get; set; } = "";
        public string ToDate { get; set; } = "";
        public string ProductName { get; set; } = "";
        public int Publised { get; set; }

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ManuFactureId { get; set; }

        public bool IsActive { get; set; }

        public long InventoryId { get; set; }

        public int StockQuantity { get; set; }
        public int AvailabilityRange { get; set; }

        public int MinQnty { get; set; }
        public int MaxQnty { get; set; }
        public bool NotReturnable { get; set; }
        public bool DisplayStockQuantity { get; set; }
        public int LowQnty { get; set; }
        public bool Returnable { get; set; }
        public string ControllerName { get; set; } = "";
        public string ActionName { get; set; } = "";

        public long ProductId { get; set; }
        public string ProductNameEn { get; set; } = "";
        public string ProductNameAr { get; set; } = "";
        public string UPCBarCode { get; set; } = "";
        public decimal sellngPrice { get; set; }
        public string BranchNameAr { get; set; } = "";
        public string BranchNameEn { get; set; } = "";
        public string variant { get; set; } = "";
        public string MainImage { get; set; } = "";
        public DateTime CreatedOn { get; set; }
        public DateTime Date { get; set; }
        public int BranchSubCategoryId { get; set; }
        public int SortId { get; set; }
        public long CartId { get; set; }
        public int FlagId { get; set; }
        public JArray jarr { get; set; }

        public string Day { get; set; } = "";
        public long ProductManagementId { get; set; }
        public string TimingType { get; set; } = "";
        public int OrderAcceptedPerHour { get; set; }
        public Decimal StartTime { get; set; }
        public Decimal EndTime { get; set; }
        public long SlotId { get; set; }
        public string TimeSlot { get; set; } = "";
        public int Acceptedorders { get; set; }
        public int TotalAcceptOrders { get; set; }
        public int ServiceId { get; set; }



        public int OrderType { get; set; }
        public int ServiceTypeId { get; set; }
        public long ServiceProviderId { get; set; }
        public DateTime RequestDate { get; set; }
        public long TimeSlotId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int filterId { get; set; }
        public string USession { get; set; } = "";
        public string Remarks { get; set; } = "";

        public string DescriptionEn { get; set; } = "";
        public string DescriptionAr { get; set; } = "";

        public string MetaDescriptionEn { get; set; } = "";
        public string MetaDescriptionAr { get; set; } = "";

        public string TagsEn { get; set; } = "";
        public string TagsAr { get; set; } = "";
        public string Images { get; set; } = "";
        public string SearchText { get; set; } = "";
        public string SkuIds_Invertory_Json { get; set; } = "";
    }

    public class UserIdDTO
    {
        public long UserId { get; set; }
    }

    //public class ProductsInfoDTOTemp
    //{

    //    public long OrderId { get; set; }

    //    public int WeekDayId { get; set; }

    //    public string PickupInServiceWeekdays { get; set; }
    //    public string DeliveryHomeWeekdays { get; set; }

    //    public long UserId { get; set; }
    //    public long ProductInventoryId { get; set; }
    //    //public string ControllerName { get; set; }
    //    //public string ActionName { get; set; }
    //    public JObject Data { get; set; }
    //    public int BranchId { get; set; }
    //    public int StoreId { get; set; }
    //    public string datasetxml { get; set; }

    //    public string SkuId { get; set; }
    //    public int LowStock { get; set; }

    //    public string message { get; set; }

    //    public string ProductInfoJson { get; set; }
    //    public string MatrixJson { get; set; }
    //    public string InventoryJson { get; set; }
    //    public string ImagesJson { get; set; }

    //    public IEnumerable<ProductsInfoDTO> SkuIds_dt { get; set; }
    //    public IEnumerable<ProductsInfoDTO> Images_dt { get; set; }

    //    public string base64 { get; set; }
    //    public string FileName { get; set; }
    //    public string FileUploadLocation { get; set; }
    //    public byte[] Bytes { get; set; }

    //    public int Id_datatable { get; set; }

    //    public FileAttribs _URLLocation { get; set; }

    //    public int Id { get; set; }

    //    public string return_url { get; set; }
    //    public long CreatedBy { get; set; }

    //    public int pagingNumber { get; set; }
    //    public int PageNumber { get; set; }
    //    public long TotalRecords { get; set; }
    //    public int PageSize { get; set; }

    //    public string FromDate { get; set; }
    //    public string ToDate { get; set; }
    //    public string ProductName { get; set; }
    //    public int Publised { get; set; }

    //    public int CategoryId { get; set; }
    //    public int SubCategoryId { get; set; }
    //    public int ManuFactureId { get; set; }

    //    public bool IsActive { get; set; }

    //    public long InventoryId { get; set; }

    //    public int StockQuantity { get; set; }
    //    public int AvailabilityRange { get; set; }

    //    public int MinQnty { get; set; }
    //    public int MaxQnty { get; set; }
    //    public bool NotReturnable { get; set; }
    //    public bool DisplayStockQuantity { get; set; }
    //    public int LowQnty { get; set; }
    //    //public bool Returnable { get; set; }
    //    //public string ControllerName { get; set; }
    //    //public string ActionName { get; set; }

    //    //public long ProductId { get; set; }
    //    //public string ProductNameEn { get; set; }
    //    //public string ProductNameAr { get; set; }
    //    //public string UPCBarCode { get; set; }
    //    //public decimal sellngPrice { get; set; }
    //    public string BranchNameAr { get; set; }
    //    public string BranchNameEn { get; set; }
    //    public string variant { get; set; }
    //    public string MainImage { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //    public DateTime Date { get; set; }
    //    public int BranchSubCategoryId { get; set; }
    //    public int SortId { get; set; }
    //    public long CartId { get; set; }
    //    public int FlagId { get; set; }
    //    public JArray jarr { get; set; }

    //}
}
