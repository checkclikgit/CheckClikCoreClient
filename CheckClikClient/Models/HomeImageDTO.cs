namespace CheckClikClient.Models
{
    public class HomeImageDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public DateTime ActivateDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsItActive { get; set; }
        public bool IsItDefault { get; set; }

        public string SearchText { get; set; }
        public IEnumerable<HomeImageDTO> HomeImageList { get; set; }
        public string Photo { get; set; }
        public string UrlPath { get; set; }

        public int PageNumber { get; set; } //startRowIndex
        public int PageSize { get; set; } //maximumRows
        public long TotalRecords { get; set; }
        public int PagingNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
