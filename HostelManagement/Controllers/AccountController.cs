using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HostelManagement.Models;

namespace HostelManagement.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext db = new AppDbContext();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Login(string username, string password)
        //{
        //    var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        //    if (user != null)
        //    {
        //        Session["UserId"] = user.UserId;
        //        Session["UserRole"] = user.Role;
        //        Session["Username"] = user.Username;

        //        if (user.Role == "Admin") return RedirectToAction("Admin", "Dashboard");
        //        return RedirectToAction("Student", "Dashboard");
        //    }
        //    ViewBag.Error = "Invalid Credentials";
        //    return View();
        //}

        [HttpPost]
        public ActionResult Login(User user)
        {
            // Check if Username, Password, AND Role all perfectly match the database
            var loggedInUser = db.Users.FirstOrDefault(u =>
                u.Username == user.Username &&
                u.Password == user.Password &&
                u.Role == user.Role); // <-- THIS IS THE CRITICAL NEW CHECK!

            if (loggedInUser != null)
            {
                // Valid user AND valid role! Set the session variables.
                Session["UserId"] = loggedInUser.UserId;
                Session["UserRole"] = loggedInUser.Role;
                Session["Username"] = loggedInUser.Username;

                // Route them to their specific dashboard
                if (loggedInUser.Role == "Admin")
                {
                    return RedirectToAction("Admin", "Dashboard");
                }
                else if (loggedInUser.Role == "Student")
                {
                    return RedirectToAction("Student", "Dashboard");
                }
            }

            // If anything fails (wrong password OR wrong role portal), reject them
            ViewBag.ErrorMessage = "Invalid username, password, or login portal.";
            return View(user);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}