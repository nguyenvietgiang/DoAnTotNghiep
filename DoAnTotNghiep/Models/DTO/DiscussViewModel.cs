using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.DTO
{
    public class DiscussViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chủ đề")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung bài viết")]
        public string? Content { get; set; }
    }
}
