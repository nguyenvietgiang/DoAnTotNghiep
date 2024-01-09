using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.DTO
{
    public class JobApplyFormDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid JobPostingID { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public IFormFile CVFile { get; set; }
    }
}
