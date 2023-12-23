using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Like
    {
        [Key]
        public Guid ID { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public Account? Account { get; set; }

        [ForeignKey("DiscussID")]
        public Guid DiscussID { get; set; }
        public Discuss? Discuss { get; set; }
    }
}
