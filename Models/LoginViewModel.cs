// File: Models/LoginViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace B_M.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email hoặc tên đăng nhập")]
        [Display(Name = "Email hoặc Tên đăng nhập")]
        public string EmailOrUsername { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ tôi")]
        public bool RememberMe { get; set; }
    }
}