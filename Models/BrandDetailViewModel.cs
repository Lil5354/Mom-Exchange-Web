// Models/BrandDetailViewModel.cs
using System.Collections.Generic;

namespace B_M.Models
{
    public class BrandDetailViewModel
    {
        public Brand BrandInfo { get; set; } // Thông tin của nhãn hàng
        public List<Product> Products { get; set; } // Danh sách sản phẩm của nhãn hàng đó
    }
}