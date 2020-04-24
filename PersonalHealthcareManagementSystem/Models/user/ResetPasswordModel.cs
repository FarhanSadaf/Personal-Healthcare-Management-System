using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalHealthcareManagementSystem.Models.user
{
    public class ResetPasswordModel
    {
        [Display(Name = "New password")]
        [Required(ErrorMessage = "New Password required", AllowEmptyStrings = false)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "Password fields doesn't match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }
    }
}