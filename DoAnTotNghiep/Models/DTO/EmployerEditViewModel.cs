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
        public string? Location { get; set; }
        public string? Descrpitons { get; set; }
    }
}
