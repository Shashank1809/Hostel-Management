using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HostelManagement.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int UserId { get; set; } // Links to the Student

        [Required]
        public int RoomId { get; set; } // Links to the Room

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Active"; // e.g., Active, Pending, Cancelled

        public virtual User User { get; set; }
        public virtual Room Room { get; set; }
    }
}