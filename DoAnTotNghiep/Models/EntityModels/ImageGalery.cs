using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class ImageGalery
    {
        [Key]
        public Guid JobPostingID { get; set; }

        [ForeignKey("EmployerID")]
        public Guid EmployerID { get; set; }
        public Employer Employer { get; set; }
        public string? ImgUrl { get; set; }
    }
}
 