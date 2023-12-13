using DoAnTotNghiep.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.DTO
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Loại tài khoản")]
        public AccountRole AccountRole { get; set; }
    }
}
