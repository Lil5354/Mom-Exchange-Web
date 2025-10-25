using B_M.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace B_M.Data.Repositories
{
    public class SocialPostRepository : Repository<SocialPost>
    {
        public SocialPostRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Get posts by author
        public IEnumerable<SocialPost> GetByAuthor(string authorName)
        {
            return _dbSet.Where(p => p.AuthorName == authorName);
        }

        // Get most liked posts
        public IEnumerable<SocialPost> GetMostLiked(int count = 10)
        {
            return _dbSet.OrderByDescending(p => p.LikeCount).Take(count);
        }

        // Get most commented posts
        public IEnumerable<SocialPost> GetMostCommented(int count = 10)
        {
            return _dbSet.OrderByDescending(p => p.CommentCount).Take(count);
        }

        // Search posts
        public IEnumerable<SocialPost> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAll();

            return _dbSet.Where(p => p.Content.Contains(searchTerm));
        }
    }
}
