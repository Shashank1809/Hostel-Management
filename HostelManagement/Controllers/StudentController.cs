using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HostelManagement.Models;

namespace HostelManagement.Controllers
{
    public class StudentController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // Helper method to ensure only Admins can access these pages
        private bool IsAdmin()
        {
            return Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin";
        }

        // 1. VIEW: List all Students
        public ActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            // Only fetch users who are marked as Students
            var students = db.Users.Where(u => u.Role == "Student").ToList();
            return View(students);
        }

        // 2. UPDATE (GET): Load the student's current data into the form
        public ActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var student = db.Users.Find(id);
            if (student == null) return HttpNotFound();

            return View(student);
        }

        // 2. UPDATE (POST): Save the modified data back to SQL
        [HttpPost]
        public ActionResult Edit(User student)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                // Force the role to stay as Student just in case
                student.Role = "Student";
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // 3. DELETE (GET): Show confirmation page
        //public ActionResult Delete(int id)
        //{
        //    if (!IsAdmin()) return RedirectToAction("Login", "Account");

        //    var student = db.Users.Find(id);
        //    if (student == null) return HttpNotFound();

        //    return View(student);
        //}

        //// 3. DELETE (POST): Actually remove them from the database
        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    if (!IsAdmin()) return RedirectToAction("Login", "Account");

        //    var student = db.Users.Find(id);
        //    db.Users.Remove(student);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}


        // 3. DELETE (GET): Show confirmation page
        public ActionResult Delete(int? id)
        {
            // 1. Hard check for Admin session
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Prevent routing crashes if ID is missing
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // 3. Find the student
            var student = db.Users.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        // 3. DELETE (POST): Actually remove them from the database
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var student = db.Users.Find(id);
            if (student != null)
            {
                db.Users.Remove(student);
                db.SaveChanges();
            }

            // Explicitly route back to the Student list
            return RedirectToAction("Index", "Student");
        }
    }
}