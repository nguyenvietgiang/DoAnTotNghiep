using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models.DTO
{
    public class ImageGaleryCreateDTO
    {
        [ForeignKey("EmployerID")]
        public Guid EmployerID { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
