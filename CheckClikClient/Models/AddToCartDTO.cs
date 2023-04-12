namespace CheckClikClient.Models
{
    //kamrul
    public class AddToCartDTO
    {
        public int StockQuantity { get; set; }
        public long ProductInventoryId { get; set; }
        public int StoreId { get; set; }
        public int BranchId { get; set; }
        public long UserId { get; set; }
        public int OrderType { get; set; }
    }
}
