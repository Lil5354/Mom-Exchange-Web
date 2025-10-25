// Models/Brand.cs
using System.Collections.Generic;

namespace B_M.Models.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; } // Đường dẫn đến file logo
        
        // Foreign key to User table for brand account
        public int? UserId { get; set; }
        
        // Navigation properties
        public virtual ICollection<Product> Products { get; set; }
        public virtual User User { get; set; } // Navigation to User account
        
        public Brand()
        {
            Products = new List<Product>();
        }
    }
}