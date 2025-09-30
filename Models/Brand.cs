﻿// Models/Brand.cs

namespace MomExchange.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; } // Đường dẫn đến file logo
    }
}