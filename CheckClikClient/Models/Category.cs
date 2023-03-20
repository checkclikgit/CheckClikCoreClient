namespace CheckClikClient.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string CategoryNameEn { get; set; }
        public string CategoryNameAr { get; set; }
        public string CategoryImage { get; set; }
        public string ReturnDays { get; set; }
    }
}
