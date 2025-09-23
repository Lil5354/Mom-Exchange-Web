// Models/CategoryViewModel.cs
using System.Collections.Generic;

namespace MomExchange.Models
{
    public class CategoryViewModel
    {
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}