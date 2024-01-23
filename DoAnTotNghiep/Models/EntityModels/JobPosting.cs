using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class JobPosting
    {
        [Key]
        public Guid JobPostingID { get; set; }
        // tiêu đề
        [Required]
        public string Title { get; set; }
        // mô tả công việc
        public string Description { get; set; }
        [ForeignKey("EmployerID")]
        public Guid EmployerID { get; set; }
        public Employer Employer { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        public string? Requirements { get; set; }
        // số lượng tuyển dụng
        public int Number { get; set; }
        // mức lương
        public int Salary { get; set; }
        [Required]
        public string position { get; set; }
        // quyền lợi
        public string benefits { get; set; }
        public string WorkingTime { get; set; }
        public bool Status { get; set; }
        public List<JobApplyForm> JobApplyForms { get; set; }

    }
}
