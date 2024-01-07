using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class JobApplyForm
    {
        [Key]
        public Guid JobApplyID { get; set; }
        public Guid JobPostingID { get; set; }
        public JobPosting JobPosting { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string CVFile { get; set; }

        public bool Status { get; set; }
    }
}
