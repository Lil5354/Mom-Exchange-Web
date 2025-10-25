using B_M.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace B_M.Data.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

                // Get products by brand
                public IEnumerable<Product> GetByBrand(string brandName)
                {
                    return _dbSet.Include("Brand").Include("ProductImages").Where(p => p.Brand.Name == brandName);
                }

                // Get products by brand ID
                public IEnumerable<Product> GetByBrandId(int brandId)
                {
                    return _dbSet.Include("Brand").Include("ProductImages").Where(p => p.BrandId == brandId);
                }

        // Get products by category
        public IEnumerable<Product> GetByCategory(string category)
        {
            return _dbSet.Where(p => p.Category == category);
        }

        // Search products
        public IEnumerable<Product> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAll();

            return _dbSet.Include("Brand").Include("ProductImages").Where(p => 
                p.Name.Contains(searchTerm) || 
                p.Brand.Name.Contains(searchTerm) ||
                p.Category.Contains(searchTerm) ||
                p.ShortDescription.Contains(searchTerm));
        }

        // Get products by seller
        public IEnumerable<Product> GetBySeller(string sellerName)
        {
            return _dbSet.Where(p => p.SellerName == sellerName);
        }

        // Get products by location
        public IEnumerable<Product> GetByLocation(string location)
        {
            return _dbSet.Where(p => p.Location.Contains(location));
        }
    }
}
