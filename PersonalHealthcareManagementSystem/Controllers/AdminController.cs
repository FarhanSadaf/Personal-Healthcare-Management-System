using PersonalHealthcareManagementSystem.Models;
using PersonalHealthcareManagementSystem.Models.admin;
using PersonalHealthcareManagementSystem.Models.specialist;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PersonalHealthcareManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private PHMSDbContext db = new PHMSDbContext();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLogin admin)
        {
            var v = db.Admins.Where(a => a.Username == admin.Username).FirstOrDefault();
            if(v != null)
            {
                if(v.Password == admin.Password)
                {
                    Session["adminId"] = v.Username;
                    Session["userId"] = null;
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError("PasswordMismatch", "Password doesn't match");
                }
            }
            ViewBag.Status = true;
            ViewBag.Message = "Admin doesn't exist";
            return View(admin);
        }

        public ActionResult Index()
        {
            if (Session["adminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var specialists = db.Specialists.ToList();
            return View(specialists);
        }

        public ActionResult Logout()
        {
            Session["adminId"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddSpecialist()
        {
            if (Session["adminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSpecialist(Specialist specialist)
        {
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(specialist.ImageFile.FileName);
                string extension = Path.GetExtension(specialist.ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                specialist.ImagePath = "~/Images/Uploaded/" + filename;
                filename = Path.Combine(Server.MapPath("~/Images/Uploaded/") + filename);
                specialist.ImageFile.SaveAs(filename);

                db.Specialists.Add(specialist);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return View(specialist);
        }

        public ActionResult DeleteSpecialist(int id)
        {
            var v = db.Specialists.Where(s => s.Id == id).FirstOrDefault();
            if(v != null)
            {
                db.Specialists.Remove(v);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DetailsSpecialist(int id)
        {
            if (Session["adminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var v = db.Specialists.Where(s => s.Id == id).FirstOrDefault();
            if (v != null)
            {
                return View(v);
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult EditSpecialist(int id)
        {
            if (Session["adminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var v = db.Specialists.Where(s => s.Id == id).FirstOrDefault();
            if (v != null)
            {
                AdminEditSpecialist specialist = new AdminEditSpecialist();
                specialist.Id = v.Id;
                specialist.Name = v.Name;
                specialist.Occupation = v.Occupation;
                specialist.Workplace = v.Workplace;
                specialist.Description = v.Description;
                specialist.Email = v.Email;
                specialist.PhoneNo = v.PhoneNo;
                specialist.ImagePath = v.ImagePath;

                return View(specialist);
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSpecialist(AdminEditSpecialist specialist)
        {
            if (ModelState.IsValid)
            {
                var v = db.Specialists.Where(s => s.Id == specialist.Id).FirstOrDefault();
                if (v != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(specialist.ImageFile.FileName);
                    string extension = Path.GetExtension(specialist.ImageFile.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                    specialist.ImagePath = "~/Images/Uploaded/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Images/Uploaded/") + filename);
                    specialist.ImageFile.SaveAs(filename);

                    v.Name = specialist.Name;
                    v.Occupation = specialist.Occupation;
                    v.Workplace = specialist.Workplace;
                    v.Description = specialist.Description;
                    v.Email = specialist.Email;
                    v.PhoneNo = specialist.PhoneNo;
                    v.ImagePath = specialist.ImagePath;

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
                //db.Entry(specialist).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return View(specialist);
        }
    }
}