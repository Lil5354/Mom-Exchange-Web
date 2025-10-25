using System.Collections.Generic;
using B_M.Models.Entities;

namespace B_M.Models.ViewModels
{
    public class CategoryViewModel
    {
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}