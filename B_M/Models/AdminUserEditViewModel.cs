using System;
using System.IO;
using System.Web.Mvc;

namespace B_M.Models
{
    internal class AdminUserEditViewModel : IView
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public byte Role { get; set; }
        public string RoleName { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public double ReputationScore { get; set; }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}