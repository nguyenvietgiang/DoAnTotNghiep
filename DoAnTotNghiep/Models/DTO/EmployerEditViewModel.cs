using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.DTO
{
    public class EmployerEditViewModel
    {
        public Guid EmployerID { get; set; }
        [Required]
        public string? CompanyName { get; set; }
        public IFormFile? NewImage { get; set; }
        public string? UrlImage { get; set; }
        public string? Industry { get; set; }
        public int? CompanySize { get; set; }
        [RegularExpression(@"^(84|0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "định dạng số điện thoại không hợp lệ.")]
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? Descrpitons { get; set; }
    }
}
