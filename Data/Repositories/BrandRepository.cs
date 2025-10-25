using B_M.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace B_M.Data.Repositories
{
    public class BrandRepository : Repository<Brand>
    {
        public BrandRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Get brands with their products
        public IEnumerable<Brand> GetBrandsWithProducts()
        {
            return _dbSet.Include("Products").ToList();
        }

        // Get brand by name
        public Brand GetByName(string name)
        {
            return _dbSet.FirstOrDefault(b => b.Name == name);
        }

        // Search brands by name or description
        public IEnumerable<Brand> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAll();

            return _dbSet.Where(b => 
                b.Name.Contains(searchTerm) || 
                b.Description.Contains(searchTerm));
        }

        // Get brand by user ID
        public Brand GetBrandByUserId(int userId)
        {
            return _dbSet.FirstOrDefault(b => b.UserId == userId);
        }
    }
}
