using PersonalHealthcareManagementSystem.Models;
using PersonalHealthcareManagementSystem.Models.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PersonalHealthcareManagementSystem.Controllers
{
    public class UserController : Controller
    {
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        //Registration POST Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified, ActivationCode")] User user)
        {
            bool Status = false;
            string Message = "";

            //Model Validation
            if (ModelState.IsValid)
            {
                #region Email Already Exists
                if (DoesEmailExist(user.EmailId))
                {
                    ModelState.AddModelError("EmailExist", "Email already exists");
                    return View(user);
                }
                #endregion

                #region Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false;

                #region Save Data to Database
                using (PHMSDbContext db = new PHMSDbContext())
                {
                    db.Users.Add(user);
                    db.SaveChanges();

                    //Send Verification Email
                    SendVerificationEmail(user.EmailId, user.ActivationCode.ToString());
                    Message = "Registration is done successfully. Account Activation link has been sent to your email: " + user.EmailId;
                    Status = true;
                }
                #endregion
            }
            else
            {
                Message = "Invalid Request";
            }
            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View(user);
        }
        
        //Other non-action functions
        [NonAction]
        public bool DoesEmailExist(string emailId)
        {
            using (PHMSDbContext db = new PHMSDbContext())
            {
                var v = db.Users.Where(u => u.EmailId == emailId).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]
        public void SendVerificationEmail(string emailId, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("frhnsadaf@gmail.com", "Personal Healthcare Management System");
            var toEmail = new MailAddress(emailId);
            var fromEmailPassword = "s@d@f01976726";

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is created successfully!";
                body = "<br/><br/>Your PHMS account is" +
                    " successfully created. Please click on the link to verify your account" +
                    "<br/><br/><a href = '" + link + "'>" + link + "</a>";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset password";
                body = "Hello, <br/><br/>We got request for changing you account password. Please click on the link below to reset your password" +
                    "<br/><br/><a href = '" + link + "'>Reset password</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        //Verify Account
        public ActionResult VerifyAccount(string id)
        {
            if (id == null) id = Guid.Empty.ToString();        //Handling Guid(null) issue

            bool Status = false;
            using (PHMSDbContext db = new PHMSDbContext())
            {
                db.Configuration.ValidateOnSaveEnabled = false;     //to avoid confirm password doesn't match issue

                var v = db.Users.Where(u => u.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]  //Prevent csrf attack
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            bool Status = false;
            string Message = "";
            using (PHMSDbContext db = new PHMSDbContext())
            {
                var v = db.Users.Where(u => u.EmailId == login.EmailId).FirstOrDefault();
                if (v != null && v.IsEmailVerified)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; //525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailId, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true; //Can't be edited through JavaScript
                        Response.Cookies.Add(cookie);

                        Session["userId"] = v.EmailId;
                        Session["adminId"] = null;
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        Status = true;
                        ModelState.AddModelError("PasswordMismatch", "Password doesn't match");
                        Message = "Invalid credential provided";
                    }
                }
                else
                {
                    Status = true;
                    Message = "User not registered yet!";
                }
            }
            ViewBag.Status = Status;
            ViewBag.Message = Message;
            return View(login);
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            Session["userId"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        //Forgot password
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailId)
        {
            //Verify Email Id
            //Generate Reset password link
            //send Email
            string Message = "";

            using (PHMSDbContext db = new PHMSDbContext())
            {
                var account = db.Users.Where(u => u.EmailId == EmailId).FirstOrDefault();
                if (account != null)
                {
                    //Send Email for reset pasword
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationEmail(account.EmailId, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    Message = "Reset password link has been sent to your email address";
                }
                else
                {
                    Message = "Account not found";
                }
            }
            ViewBag.Message = Message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account accociated with link
            //redirect to reset password page
            using (PHMSDbContext db = new PHMSDbContext())
            {
                var user = db.Users.Where(u => u.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var Message = "";
            if (ModelState.IsValid)
            {
                using (PHMSDbContext db = new PHMSDbContext())
                {
                    var user = db.Users.Where(u => u.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = null;

                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        Message = "New password updated successfully";
                    }
                }
            }
            else
            {
                Message = "Invalid Request";
            }
            ViewBag.Message = Message;
            return View(model);
        }
        
        //Added later
        [Authorize]
        public ActionResult Details()
        {
            using (PHMSDbContext db = new PHMSDbContext())
            {
                if(Session["userId"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                string email = Session["userId"].ToString();
                var v = db.Users.Where(u => u.EmailId == email).FirstOrDefault();
                return View(v);
            }
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            using (PHMSDbContext db = new PHMSDbContext())
            {
                var v = db.Users.Where(u => u.Id == id).FirstOrDefault();
                EditUserModel user = new EditUserModel();
                user.Id = v.Id;
                user.FirstName = v.FirstName;
                user.LastName = v.LastName;
                user.DateOfBirth = v.DateOfBirth;
                return View(user);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserModel user)
        {
            try
            {
                using (PHMSDbContext db = new PHMSDbContext())
                {
                    var v = db.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                    if (v.Password == Crypto.Hash(user.PreviousPassword))
                    {
                        v.FirstName = user.FirstName;
                        v.LastName = user.LastName;
                        v.DateOfBirth = user.DateOfBirth;
                        v.Password = Crypto.Hash(user.Password);
                        v.ConfirmPassword = Crypto.Hash(user.Password);

                        db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordMismatch", "Password doesn't match");
                        return View(user);
                    }
                }
            }
            catch (Exception)
            {
                HttpNotFound();
            }
            return RedirectToAction("Details");
        }
    }
}