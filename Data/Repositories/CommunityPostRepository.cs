using B_M.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B_M.Data.Repositories
{
    public class CommunityPostRepository : Repository<CommunityPost>
    {
        public CommunityPostRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Get posts by author
        public IEnumerable<CommunityPost> GetByAuthor(string authorName)
        {
            return _dbSet.Where(p => p.AuthorName == authorName);
        }

        // Get recent posts
        public IEnumerable<CommunityPost> GetRecent(int count = 10)
        {
            return _dbSet.OrderByDescending(p => p.PostDate).Take(count);
        }

        // Search posts
        public IEnumerable<CommunityPost> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAll();

            return _dbSet.Where(p => 
                p.Title.Contains(searchTerm) || 
                p.Content.Contains(searchTerm) ||
                p.Excerpt.Contains(searchTerm));
        }

        // Get posts by tag
        public IEnumerable<CommunityPost> GetByTag(string tag)
        {
            return _dbSet.Where(p => p.Tags.Contains(tag));
        }

        // Get posts by date range
        public IEnumerable<CommunityPost> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return _dbSet.Where(p => p.PostDate >= startDate && p.PostDate <= endDate);
        }
    }
}
