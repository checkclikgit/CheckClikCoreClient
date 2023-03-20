namespace CheckClikClient.Models
{
    public class AdminMenuCategoryDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }
        public string Url { get; set; }
        public bool TargetNewPage { get; set; }
        public int SortIndex { get; set; }
    }
}
