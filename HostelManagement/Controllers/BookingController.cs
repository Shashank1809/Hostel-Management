using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HostelManagement.Models;

namespace HostelManagement.Controllers
{
    public class BookingController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Booking/Index
        public ActionResult Index()
        {
            // 1. Security Check: Only Admins can see all bookings
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Fetch the bookings AND include the related User and Room data
            // (Assuming your Booking model has navigation properties named 'User' and 'Room')
            //var bookings = db.Bookings
            //                 .Include(b => b.UserId)
            //                 .Include(b => b.RoomId)
            //                 .OrderByDescending(b => b.BookingDate) // Newest first
            //                 .ToList();

            // Fetch the bookings without trying to forcefully Include the missing objects
            //var bookings = db.Bookings
            //                 .OrderByDescending(b => b.BookingDate) // Newest first
            //                 .ToList();

            var bookings = db.Bookings
                             .Include(b => b.User)
                             .Include(b => b.Room)
                             .OrderByDescending(b => b.BookingDate)
                             .ToList();

            return View(bookings);
        }
    }
}