﻿// Models/ViewModels/BrandDetailViewModel.cs
using System.Collections.Generic;
using B_M.Models.Entities;

namespace B_M.Models.ViewModels
{
    public class BrandDetailViewModel
    {
        public Brand BrandInfo { get; set; } // Thông tin của nhãn hàng
        public List<Product> Products { get; set; } // Danh sách sản phẩm của nhãn hàng đó
    }
}