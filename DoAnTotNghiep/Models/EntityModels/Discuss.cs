using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    // đăng bài thảo luận lên diễn đàn
    public class Discuss
    {
        [Key]
        public Guid DiscussID { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung bài viết")]
        public string? Content { get; set; }
        public string? Type { get; set; }
        public Guid UserId { get; set; }
        public Account? Account { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public int LikeCount { get; set; } = 0;
        public List<Like>? Likes { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
