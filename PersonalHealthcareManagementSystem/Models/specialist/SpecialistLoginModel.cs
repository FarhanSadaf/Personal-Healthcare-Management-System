using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalHealthcareManagementSystem.Models.specialist
{
    public class SpecialistLoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your ID")]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}