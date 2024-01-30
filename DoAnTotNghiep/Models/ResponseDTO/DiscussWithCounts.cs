using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Models.ResponseDTO
{
    public class DiscussWithCounts
    {
        public Discuss Discuss { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
