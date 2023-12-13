using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.DTO
{
    public class LoginModels
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
