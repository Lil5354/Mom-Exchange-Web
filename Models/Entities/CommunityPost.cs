// Models/CommunityPost.cs
using System;
using System.Collections.Generic;

namespace B_M.Models.Entities
{
    public class CommunityPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatarUrl { get; set; }
        public DateTime PostDate { get; set; }
        public string ImageUrl { get; set; }
        public string Excerpt { get; set; } // Đoạn trích ngắn
        public string Content { get; set; } // Nội dung đầy đủ
        public List<string> Tags { get; set; }
    }
}