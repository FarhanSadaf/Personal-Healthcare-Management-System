using PersonalHealthcareManagementSystem.Models.admin;
using PersonalHealthcareManagementSystem.Models.all;
using PersonalHealthcareManagementSystem.Models.message;
using PersonalHealthcareManagementSystem.Models.specialist;
using PersonalHealthcareManagementSystem.Models.user;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PersonalHealthcareManagementSystem.Models
{
    public class PHMSDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<AdminLogin> Admins { get; set; }
        public DbSet<Specialist> Specialists { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}