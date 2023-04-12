namespace CheckClikClient.Models
{
    public class PlaceOrderDTO
    {
        public long UserId { get; set; }
        public string Comments { get; set; }
        public long AddressId { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public int OrderType { get; set; }
        public DateTime ExpectingDelivery { get; set; }
        public long TimeSlotId { get; set; }
        public bool PaymentStatus { get; set; }
    }
}
