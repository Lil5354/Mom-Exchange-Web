// Models/SocialPost.cs
using System;
using System.Collections.Generic;

namespace B_M.Models.Entities // Đảm bảo namespace này khớp với dự án của bạn
{
    public class SocialPost
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatarUrl { get; set; }
        public string PostTime { get; set; } // Ví dụ: "2 giờ trước"
        public string Content { get; set; }
        public string ImageUrl { get; set; } // URL ảnh trong bài đăng
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}