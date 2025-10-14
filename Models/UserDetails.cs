// File: Models/UserDetails.cs
using System.ComponentModel.DataAnnotations;

namespace MomExchange.Models
{
    public class UserDetails
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(500)]
        public string ProfilePictureURL { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public double ReputationScore { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}

