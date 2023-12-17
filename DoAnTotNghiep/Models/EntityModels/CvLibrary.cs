using DoAnTotNghiep.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class CvLibrary
    {
        [Key]
        public Guid CvID { get; set; }

        [Required]
        public string CvName { get; set; }

        [Required]
        public string CvType { get; set; }

        [Required]
        public string CvImage { get; set; } 

        [Required]
        public string CvFile { get; set; }
    }
}
