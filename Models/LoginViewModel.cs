// File: Models/LoginViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace MomExchange.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email của bạn")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ tôi")]
        public bool RememberMe { get; set; }
    }
}