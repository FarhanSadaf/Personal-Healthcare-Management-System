using PersonalHealthcareManagementSystem.Models;
using PersonalHealthcareManagementSystem.Models.message;
using PersonalHealthcareManagementSystem.Models.specialist;
using PersonalHealthcareManagementSystem.Models.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalHealthcareManagementSystem.Controllers
{
    public class MessageController : Controller
    {
        private PHMSDbContext db = new PHMSDbContext();

        //User
        [Authorize]
        public ActionResult ToSpecialist(int id)
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string email = Session["userId"].ToString();
            var from = db.Users.Where(u => u.EmailId == email).FirstOrDefault();
            var to = db.Specialists.Where(s => s.Id == id).FirstOrDefault();
            ViewBag.ToName = to.Name;

            var messages = db.Messages.Where(m => (m.From == from.Id && m.To == to.Id) || (m.From == to.Id && m.To == from.Id)).ToList();
            if (messages.Count > 0)
            {
                ViewBag.Status = true;
                ViewBag.Messages = messages;
                ViewBag.ToImg = to.ImagePath;
            }

            Message message = new Message();
            message.From = from.Id;
            message.To = to.Id;
            message.SentByUser = true;
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToSpecialist(Message message)
        {
            message.TimeSent = DateTime.Now;
            db.Messages.Add(message);
            db.SaveChanges();
            return RedirectToAction("ToSpecialist", new { id = message.To });
        }

        //Specialist
        public ActionResult PopulateFromUsers(int id)
        {
            return RedirectToAction("FromUsers", new { selectedUserId = id });
        }
        public ActionResult FromUsers(int? selectedUserId)
        {
            if (Session["specialistId"] == null)
            {
                return RedirectToAction("Login", "Specialist");
            }

            var toUsers = new List<User>();

            int id = Convert.ToInt32(Session["specialistId"]);
            var from = db.Specialists.Where(s => s.Id == id).FirstOrDefault();

            var messagesToSpecialist = db.Messages.Where(m => m.To == from.Id && m.SentByUser == true).ToList();
            foreach (var item in messagesToSpecialist)
            {
                var v = db.Users.Where(u => u.Id == item.From).FirstOrDefault();
                if (!toUsers.Contains(v))
                {
                    toUsers.Add(v);
                }
            }

            ViewBag.ToUsers = toUsers;

            //show messages of selected user
            if (selectedUserId != null && selectedUserId != 0)
            {
                var to = toUsers.Where(u => u.Id == selectedUserId).FirstOrDefault();
                ViewBag.ToName = to.FirstName + " " + to.LastName;
                var msges = db.Messages.Where(m => (m.From == from.Id && m.To == to.Id) || (m.From == to.Id && m.To == from.Id)).ToList();
                if (msges.Count > 0)
                {
                    ViewBag.Status = true;
                    ViewBag.Messages = msges;
                    ViewBag.FromImg = from.ImagePath;
                }

                Message msg = new Message();
                msg.From = from.Id;
                msg.To = to.Id;
                msg.SentByUser = false;
                return View(msg);
            }

            //show messages of first user by default
            ViewBag.ToName = toUsers[0].FirstName + " " + toUsers[0].LastName;

            var toId = toUsers[0].Id;
            var messages = db.Messages.Where(m => (m.From == toId && m.To == from.Id) || (m.From == from.Id && m.To == toId)).ToList();
            if (messages.Count > 0)
            {
                ViewBag.Status = true;
                ViewBag.Messages = messages;
                ViewBag.FromImg = from.ImagePath;
            }

            Message message = new Message();
            message.From = from.Id;
            message.To = toUsers[0].Id;
            message.SentByUser = false;
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FromUsers(Message message)
        {
            message.TimeSent = DateTime.Now;
            db.Messages.Add(message);
            db.SaveChanges();
            return RedirectToAction("FromUsers", new { selectedUserId = message.To });
        }

        //testing
        //User
        public ActionResult PopulateFromSpecialists(int id)
        {
            return RedirectToAction("FromSpecialists", new { selectedSpecialistId = id });
        }
        [Authorize]
        public ActionResult FromSpecialists(int? selectedSpecialistId)
        {
            var toSpecialists = new List<Specialist>();

            string email = Session["userId"].ToString();
            var from = db.Users.Where(s => s.EmailId == email).FirstOrDefault();

            var messagesToUser = db.Messages.Where(m => m.To == from.Id && m.SentByUser == false).ToList();
            foreach (var item in messagesToUser)
            {
                var v = db.Specialists.Where(s => s.Id == item.From).FirstOrDefault();
                if (!toSpecialists.Contains(v))
                {
                    toSpecialists.Add(v);
                }
            }

            ViewBag.ToUsers = toSpecialists;

            //show messages of selected user
            if (selectedSpecialistId != null && selectedSpecialistId != 0)
            {
                var to = toSpecialists.Where(u => u.Id == selectedSpecialistId).FirstOrDefault();
                ViewBag.ToName = to.Name;
                var msges = db.Messages.Where(m => (m.From == from.Id && m.To == to.Id) || (m.From == to.Id && m.To == from.Id)).ToList();
                if (msges.Count > 0)
                {
                    ViewBag.Status = true;
                    ViewBag.Messages = msges;
                    ViewBag.FromImg = to.ImagePath;
                }

                Message msg = new Message();
                msg.From = from.Id;
                msg.To = to.Id;
                msg.SentByUser = true;
                return View(msg);
            }

            //show messages of first user by default
            ViewBag.ToName = toSpecialists[0].Name;

            var toId = toSpecialists[0].Id;
            var messages = db.Messages.Where(m => (m.From == toId && m.To == from.Id) || (m.From == from.Id && m.To == toId)).ToList();
            if (messages.Count > 0)
            {
                ViewBag.Status = true;
                ViewBag.Messages = messages;
                ViewBag.FromImg = toSpecialists[0].ImagePath;
            }

            Message message = new Message();
            message.From = from.Id;
            message.To = toSpecialists[0].Id;
            message.SentByUser = true;
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FromSpecialists(Message message)
        {
            message.TimeSent = DateTime.Now;
            db.Messages.Add(message);
            db.SaveChanges();
            return RedirectToAction("FromSpecialists", new { selectedSpecialistId = message.To });
        }

    }
}