using System.Collections.Generic;

namespace B_M.Models
{
    public class AdminUsersViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public string RoleFilter { get; set; }
    }
}

