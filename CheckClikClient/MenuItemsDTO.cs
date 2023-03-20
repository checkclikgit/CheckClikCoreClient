using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckClickClient.Models
{
    public class MenuItemsDTO
    {
        public int Id { get; set; }
        public int MenuCategoryId { get; set; }
        public int MenuItemId { get; set; }
        
        public string Name { get; set; }
        public string MenuItemName { get; set; }
        
        public string Icon { get; set; }
        public string Url { get; set; }        
        public bool IsActive { get; set; }
        public bool isSelected { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int64 ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public Int64 DeletedBy { get; set; }
    }
}