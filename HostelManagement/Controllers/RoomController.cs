using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HostelManagement.Models;

namespace HostelManagement.Controllers
{
    public class RoomController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // Admin and Student View
        public ActionResult Index()
        {
            return View(db.Rooms.ToList());
        }

        // Admin Only Add
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        // GET: Room/Edit/5
        public ActionResult Edit(int? id)
        {
            // Ensure the user is an Admin
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            // Find the room in the database
            var room = db.Rooms.Find(id);
            if (room == null) return HttpNotFound();

            return View(room);
        }

        // POST: Room/Edit/5
        [HttpPost]
        public ActionResult Edit(Room room)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Tell Entity Framework this record has been modified
                db.Entry(room).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        // GET: Room/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var room = db.Rooms.Find(id);
            if (room == null) return HttpNotFound();

            return View(room);
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Room/Book/5 (Show the booking confirmation page)
        public ActionResult Book(int? id)
        {
            // Ensure the user is actually logged in as a Student
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var room = db.Rooms.Find(id);
            if (room == null || !room.IsAvailable)
            {
                return HttpNotFound(); // Prevent booking a room that doesn't exist or is already full
            }

            return View(room);
        }

        // POST: Room/Book/5 (Process the booking)
        [HttpPost, ActionName("Book")]
        public ActionResult BookConfirmed(int id)
        {
            // 1. Security Check
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Get the logged-in Student's ID
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account"); // Force login if session dropped
            }

            int currentStudentId = Convert.ToInt32(Session["UserId"]);

            var room = db.Rooms.Find(id);

            if (room != null && room.IsAvailable)
            {
                // 3. Mark the room as occupied
                room.IsAvailable = false;
                db.Entry(room).State = System.Data.Entity.EntityState.Modified;

                // 4. Create the new Booking record
                var newBooking = new HostelManagement.Models.Booking
                {
                    RoomId = room.RoomId,          // The room being booked
                    UserId = currentStudentId,     // The student booking it
                    BookingDate = System.DateTime.Now,
                    Status = "Active"              // Or whatever status columns your model uses
                };

                // Add it to the Bookings table
                db.Bookings.Add(newBooking);

                // 5. Save EVERYTHING to the database at once
                db.SaveChanges();
            }

            // Send the student back to the room list
            return RedirectToAction("Index");
        }
    }
}