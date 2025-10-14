// File: Models/User.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace B_M.Models
{
    public class User
    {
        public int UserID { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // Role: 1 = Admin, 2 = Mom, 3 = Brand
        public byte Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation property
        public UserDetails UserDetails { get; set; }
    }
}

