using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HostelManagement.Models;

namespace HostelManagement.Controllers
{
    public class RentController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Rent
        public ActionResult Index()
        {
            // Ensure the user is logged in
            if (Session["UserRole"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // If Admin, show all rents. If Student, show only their rent.
            if (Session["UserRole"].ToString() == "Admin")
            {
                return View(); // In a full implementation, pass all rent records here
            }
            else
            {
                return View(); // In a full implementation, pass only this student's rent records
            }
        }
    }
}