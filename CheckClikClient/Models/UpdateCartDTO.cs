namespace CheckClikClient.Models
{
    public class UpdateCartDTO
    {
        public long CartId { get; set; }
        public int StockQuantity { get; set; }
        public int FlagId { get; set; }
    }
}
