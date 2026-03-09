using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;

namespace HostelManagement.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        [Required]
        public string RoomType { get; set; } // e.g., Single, Double
        public string Description { get; set; }
        public decimal MonthlyRent { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}