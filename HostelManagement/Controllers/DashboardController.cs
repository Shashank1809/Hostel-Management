using HostelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
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

        // GET: Dashboard/Student
        public ActionResult Student()
        {
            // 1. Security Check: Ensure the user is logged in as a Student
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Safely grab the logged-in Student's ID
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int studentId = Convert.ToInt32(Session["UserId"]);

            // 3. Fetch ONLY the bookings belonging to this specific student
            var myBookings = db.Bookings
                               .Include(b => b.Room) // Pull in the room details
                               .Where(b => b.UserId == studentId)
                               .OrderByDescending(b => b.BookingDate)
                               .ToList();

            return View(myBookings);
        }
    }
}