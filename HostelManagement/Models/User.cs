using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;

namespace HostelManagement.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Role { get; set; } = "Student"; // "Admin" or "Student"

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Website { get; set; }
        public string Bio { get; set; }
    }
}