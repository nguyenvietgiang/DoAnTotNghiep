using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Employer
    {
        [Key]
        public Guid EmployerID { get; set; }

        [Required]
        public string CompanyName { get; set; }
        public string? UrlImage { get; set; }
        public string? Industry { get; set; } 
        public int? CompanySize { get; set; }
        public string? Location { get; set; }

        [ForeignKey("EmployerID")]
        public Account Account { get; set; }
    }
}
