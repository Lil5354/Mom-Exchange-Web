using B_M.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace B_M.Data.Repositories
{
    public class TradePostRepository : Repository<TradePost>
    {
        public TradePostRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Get posts by seller
        public IEnumerable<TradePost> GetBySeller(string sellerName)
        {
            return _dbSet.Where(p => p.SellerName == sellerName);
        }

        // Get posts by location
        public IEnumerable<TradePost> GetByLocation(string location)
        {
            return _dbSet.Where(p => p.Location.Contains(location));
        }

        // Search posts
        public IEnumerable<TradePost> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAll();

            return _dbSet.Where(p => 
                p.DesiredItemsDescription.Contains(searchTerm) ||
                p.ItemToOffer.Name.Contains(searchTerm));
        }
    }
}
