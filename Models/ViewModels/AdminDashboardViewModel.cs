using B_M.Models.Entities;
using System.Collections.Generic;

namespace B_M.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int AdminUsers { get; set; }
        public int MomUsers { get; set; }
        public int BrandUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public List<User> RecentUsers { get; set; } = new List<User>();
    }
}

