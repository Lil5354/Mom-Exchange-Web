using System;
using System.ComponentModel.DataAnnotations;

namespace B_M.Models.ViewModels
{
    public class AdminUserEditViewModel
    {
        public int UserID { get; set; }
        
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự")]
        public string Email { get; set; }
        
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
        public string UserName { get; set; }
        
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; }
        
        [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string Address { get; set; }
        
        public bool IsActive { get; set; }
        public byte Role { get; set; }
        
        public string RoleName { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public double ReputationScore { get; set; }
    }
}
