using PersonalHealthcareManagementSystem.Models.user;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalHealthcareManagementSystem.Models.all
{
    public class TestResult
    {
        public TestResult()
        {
            TestDate = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime TestDate { get; set; }
        public float Weight { get; set; }
        public double Bmi { get; set; }
        public double Bmr { get; set; }
        public double DietPercentage { get; set; }
        public double SmokingPercentage { get; set; }
        public double AlcoholPercentage { get; set; }
        public double FitnessPercentage { get; set; }
        public double BpMap { get; set; }
        
        //1-M
        public int UserId { get; set; }
        public virtual User User { get; set; }  
    }
}