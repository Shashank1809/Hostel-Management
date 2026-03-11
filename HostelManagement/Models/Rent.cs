using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HostelManagement.Models
{
    public class Rent
    {
        public int RentId { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public string RentMonth { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }

        // Navigation Properties to link tables together
        public virtual User User { get; set; }
        public virtual Room Room { get; set; }
    }
}