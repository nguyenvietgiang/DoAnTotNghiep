using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Like
    {
        [Key]
        public Guid ID { get; set; }
        public Guid UserId { get; set; }
        public Account? Account { get; set; }

        public Guid DiscussID { get; set; }
        public Discuss? Discuss { get; set; }
    }
}
