using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckClickClient.Models
{
    public class UsersDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Emp ID cannot be empty", AllowEmptyStrings = false)]
        public int? EmpID { get; set; }
        [Required(ErrorMessage = "Full Name cannot be empty", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full Name length should be in between 3 and 100")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email Id cannot be empty", AllowEmptyStrings = false)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email")]
        //[EmailAddress(ErrorMessage = "Invalid email")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Mobile No. cannot be empty", AllowEmptyStrings = false)]
        [RegularExpression("^[5][0-9]{8}$", ErrorMessage = "Invalid Mobile number")]
        public string MobileNo { get; set; }
        public int RoleId { get; set; }
        [Required(ErrorMessage = "User Name cannot be empty", AllowEmptyStrings = false)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password cannot be empty", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "password should be between 4 and 50 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password cannot be empty", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "confirm password should be between 4 and 50 characters")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password doesn't match")]
        public string ConfirmPassword { get; set; }
        //public int RoleId { get; set; }
        public bool Status { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public long DeletedBy { get; set; }
        public bool IsDeleted { get; set; }

        public long FlagId { get; set; }
        public IEnumerable<RolesDTO> LstRoles { get; set; }
        public IEnumerable<UsersDTO> LstUsers { get; set; }      

        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }

        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string RoleNameEn { get; set; }
    }

    public class ChangePasswordDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Password cannot be empty", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "password should be between 4 and 50 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password cannot be empty", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "confirm password should be between 4 and 50 characters")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password doesn't match")]
        public string NewPassword { get; set; }
    }

}