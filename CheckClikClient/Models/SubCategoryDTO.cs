namespace CheckClikClient.Models
{
    public class SubCategoryDTO
    { 
            public int Id { get; set; }
            public int BranchId { get; set; }
            public int CategoryId { get; set; }
            public string NameEn { get; set; }
            public string NameAr { get; set; } 
            public FileAttribs ImageFileAttr { get; set; }
            public string Image { get; set; }
            public bool IsActive { get; set; }
        public int CartItemCount { get; set; }
        public int NotificationCount { get; set; }
        public int ProductCount { get; set; }

        public DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            public DateTime ModifiedOn { get; set; }
            public int ModifiedBy { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime DeletedOn { get; set; }
            public int DeletedBy { get; set; }
            public string MainCategoryEn { get; set; }
            public string MainCategoryAr { get; set; }
            public long FlagId { get; set; }
            public bool Create { get; set; }
            public bool Edit { get; set; }
            public bool Update { get; set; }
            public bool Delete { get; set; }
            public string StatusCode { get; set; }
            public string StatusMessage { get; set; }
        
    }
}
