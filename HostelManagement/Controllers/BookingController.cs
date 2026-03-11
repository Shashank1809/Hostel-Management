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

        // GET: Booking/Delete/5 (Show the confirmation page)
        public ActionResult Delete(int? id)
        {
            // Ensure the user is an Admin
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            // Fetch the booking with its related User and Room data for the warning screen
            var booking = db.Bookings
                            .Include(b => b.User)
                            .Include(b => b.Room)
                            .FirstOrDefault(b => b.BookingId == id);

            if (booking == null) return HttpNotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5 (Process the removal)
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = db.Bookings.Find(id);
            if (booking != null)
            {
                // 1. Find the connected room and make it available again
                var room = db.Rooms.Find(booking.RoomId);
                if (room != null)
                {
                    room.IsAvailable = true;
                    db.Entry(room).State = System.Data.Entity.EntityState.Modified;
                }

                // 2. Delete the booking record entirely
                db.Bookings.Remove(booking);

                // 3. Save all changes to the database
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}