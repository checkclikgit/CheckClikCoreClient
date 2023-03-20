using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace CheckClickClient.Models
{
    public class RolesDTO
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "English name is required", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Role Name English length should be in between 3 and 100")]
        public string NameEn { get; set; }
        [Required(ErrorMessage = "Arabic name is required", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Role Name Arabic length should be in between 3 and 100")]
        public string NameAr { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public long DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public long FlagId { get; set; }
        public IEnumerable<RolesDTO> RolesList { get; set; }
        public IEnumerable<MenuItemsDTO> MenuItemlist { get; set; }
        public IEnumerable<RolesDTO> RoleAccessList { get; set; }
        public string MenuItemIds { get; set; }
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<int> listSelected { get; set; }
    }
}