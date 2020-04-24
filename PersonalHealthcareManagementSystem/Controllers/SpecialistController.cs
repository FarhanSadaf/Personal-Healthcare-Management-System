using PersonalHealthcareManagementSystem.Models;
using PersonalHealthcareManagementSystem.Models.specialist;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PersonalHealthcareManagementSystem.Controllers
{
    public class SpecialistController : Controller
    {
        private PHMSDbContext db = new PHMSDbContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(SpecialistLoginModel specialist)
        {
            var v = db.Specialists.Where(s => s.Id == specialist.Id).FirstOrDefault();
            if (v != null)
            {
                if (v.Password == specialist.Password)
                {
                    Session["specialistId"] = v.Id;
                    Session["specialistName"] = v.Name;
                    Session["userId"] = null;
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", "Specialist");
                }
                else
                {
                    ModelState.AddModelError("PasswordMismatch", "Password doesn't match");
                }
            }
            ViewBag.Status = true;
            ViewBag.Message = "Specialist doesn't exist";
            return View(specialist);
        }

        public ActionResult Logout()
        {
            Session["specialistId"] = null;
            Session["specialistName"] = null; 
            Session["userId"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit()
        {
            if (Session["specialistId"] == null)
            {
                return RedirectToAction("Login", "Specialist");
            }

            int id = Convert.ToInt32(Session["specialistId"]);
            EditSpecialistModel specialist = new EditSpecialistModel();
            var v = db.Specialists.Where(s => s.Id == id).FirstOrDefault();
            if(v != null)
            {
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
            return RedirectToAction("Logout", "Specialist");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditSpecialistModel specialist)
        {
            if (ModelState.IsValid)
            {
                var v = db.Specialists.Where(s => s.Id == specialist.Id).FirstOrDefault();
                if(v != null)
                {
                    if (v.Password == specialist.PreviousPassword)
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
                        v.Password = specialist.Password;
                        v.ConfirmPassword = specialist.Password;
                        v.ImagePath = specialist.ImagePath;
                        db.SaveChanges();
                        return RedirectToAction("Details");
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordMismatch", "Password doesn't match");
                        specialist.ImagePath = v.ImagePath;
                    }
                }
            }
            return View(specialist);
        }
        
        public ActionResult Details()
        {
            if (Session["specialistId"] == null)
            {
                return RedirectToAction("Login", "Specialist");
            }

            int id = Convert.ToInt32(Session["specialistId"]);
            var v = db.Specialists.Where(s => s.Id == id).FirstOrDefault();
            return View(v);
        }

        // GET: Specialist
        public ActionResult Index()
        {
            if (Session["specialistId"] == null)
            {
                return RedirectToAction("Login", "Specialist");
            }

            //go to messages
            return RedirectToAction("FromUsers", "Message");
        }

    }
}