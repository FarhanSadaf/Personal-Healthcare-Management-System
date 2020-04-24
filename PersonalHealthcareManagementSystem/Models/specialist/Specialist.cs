using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalHealthcareManagementSystem.Models.specialist
{
    public class Specialist
    {
        [Key]
        public int Id{ get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name required")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Occupation required")]
        public string Occupation { get; set; }
        [Display(Name = "Institute / Hospital")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Workplace required")]
        public string Workplace { get; set; }
        public string Description { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No.")]
        public string PhoneNo { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        [NotMapped]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password & Comfirm password doesn't match")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Upload image")]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}