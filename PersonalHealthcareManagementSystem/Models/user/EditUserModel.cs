using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalHealthcareManagementSystem.Models.user
{
    public class EditUserModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string LastName { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "(0:dd/MM/yyyy)")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Previous password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter previous password")]
        [DataType(DataType.Password)]
        public string PreviousPassword { get; set; }
        [Display(Name = "New password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter new password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        [Display(Name = "Re-enter new password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password & Comfirm password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}