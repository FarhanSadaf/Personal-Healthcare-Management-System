using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalHealthcareManagementSystem.Models.admin
{
    public class AdminLogin
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter username")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}