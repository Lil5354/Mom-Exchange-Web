using B_M.Models.Entities;
using System.Collections.Generic;

namespace B_M.Models.ViewModels
{
    public class BrandDashboardViewModel
    {
        public Brand Brand { get; set; }
        public List<Product> Products { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int InactiveProducts => TotalProducts - ActiveProducts;
    }
}
