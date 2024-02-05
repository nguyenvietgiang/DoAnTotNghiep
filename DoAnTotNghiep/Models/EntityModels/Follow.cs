using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Follow
    { 
        [Key]
        public Guid ID { get; set; }
        public Guid UserId { get; set; }
        public Account? Account { get; set; }
        public Guid FollowUserId { get; set; }

    }
}
