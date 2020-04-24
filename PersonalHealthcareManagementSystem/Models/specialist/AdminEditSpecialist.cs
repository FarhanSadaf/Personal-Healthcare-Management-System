using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalHealthcareManagementSystem.Models.specialist
{
    public class AdminEditSpecialist
    {
        [Key]
        public int Id { get; set; }
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
        
        [Display(Name = "Upload image")]
        public string ImagePath { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}