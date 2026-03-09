using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//using Microsoft.EntityFrameworkCore;

using System.Data.Entity;

namespace HostelManagement.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        // public DbSet<RentPayment> RentPayments { get; set; } // Add when implementing Rent module
    }
}