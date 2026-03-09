using HostelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HostelManagement.Controllers
{
    public class DashboardController : Controller
    {
        private AppDbContext db = new AppDbContext();
        public ActionResult Admin()
        {
            // Security Check
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch dynamic counts from the database
            ViewBag.TotalRooms = db.Rooms.Count();
            ViewBag.TotalStudents = db.Users.Count(u => u.Role == "Student");
            ViewBag.ActiveBookings = db.Bookings.Count(b => b.Status == "Active");

            return View();
        }

        public ActionResult Student()
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Student")
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}