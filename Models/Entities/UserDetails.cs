// File: Models/UserDetails.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B_M.Models.Entities
{
    public class UserDetails
    {
        [Key]
        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(500)]
        public string ProfilePictureURL { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        public double ReputationScore { get; set; }

        // Navigation property
        [Required]
        public User User { get; set; }
    }
}

