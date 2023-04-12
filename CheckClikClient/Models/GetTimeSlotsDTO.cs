namespace CheckClikClient.Models
{
    public class GetTimeSlotsDTO
    {
        public int WeekdayId { get; set; }
        public DateTime Date { get; set; }
        public long UserId { get; set; }
        public int OrderType { get; set; }
    }
}
