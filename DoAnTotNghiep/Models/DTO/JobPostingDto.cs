using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.DTO
{
    public class JobPostingDto
    {
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ làm việc")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập yêu cầu công việc")]
        public string Requirements { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng tuyển dụng")]
        public int Number { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội mức lương")]
        public int Salary { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vị trí làm việc")]
        public string Position { get; set; }
        public string WorkTime { get; set; }
        public string Benefits { get; set; }
    }
}
