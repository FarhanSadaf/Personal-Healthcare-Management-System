using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalHealthcareManagementSystem.Models.message
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public bool SentByUser { get; set; }
        public string Text { get; set; }
        public DateTime TimeSent { get; set; }
    }
}