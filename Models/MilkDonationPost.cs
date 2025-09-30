// Models/MilkDonationPost.cs
using System;

namespace MomExchange.Models
{
    public class MilkDonationPost
    {
        public int Id { get; set; }
        public string DonorName { get; set; }
        public string Location { get; set; }
        public DateTime DateOfExpression { get; set; }
        public string DietInfo { get; set; }
        public string StorageInfo { get; set; }
        public string Note { get; set; }
        public string DonorAvatarUrl { get; set; }
        public bool IsHealthVerified { get; set; } // Thuộc tính quan trọng!
    }
}