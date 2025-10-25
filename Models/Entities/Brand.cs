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
        
        // Navigation property
        public virtual ICollection<Product> Products { get; set; }
        
        public Brand()
        {
            Products = new List<Product>();
        }
    }
}