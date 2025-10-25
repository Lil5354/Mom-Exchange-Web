using B_M.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B_M.Data.Repositories
{
    public class MilkDonationPostRepository : Repository<MilkDonationPost>
    {
        public MilkDonationPostRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Get posts by donor
        public IEnumerable<MilkDonationPost> GetByDonor(string donorName)
        {
            return _dbSet.Where(p => p.DonorName == donorName);
        }

        // Get verified posts only
        public IEnumerable<MilkDonationPost> GetVerified()
        {
            return _dbSet.Where(p => p.IsHealthVerified);
        }

        // Get posts by location
        public IEnumerable<MilkDonationPost> GetByLocation(string location)
        {
            return _dbSet.Where(p => p.Location.Contains(location));
        }

        // Get recent posts
        public IEnumerable<MilkDonationPost> GetRecent(int count = 10)
        {
            return _dbSet.OrderByDescending(p => p.DateOfExpression).Take(count);
        }

        // Search posts
        public IEnumerable<MilkDonationPost> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAll();

            return _dbSet.Where(p => 
                p.DonorName.Contains(searchTerm) ||
                p.Location.Contains(searchTerm) ||
                p.Note.Contains(searchTerm));
        }
    }
}
