using DoAnTotNghiep.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.DTO
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email là trường bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tên là trường bắt buộc.")]
        [MinLength(5, ErrorMessage = "Tên phải có ít nhất 5 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{6,}$", ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái và một chữ số.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Loại tài khoản là trường bắt buộc.")]
        [Display(Name = "Loại tài khoản")]
        public AccountRole AccountRole { get; set; }
    }
}
