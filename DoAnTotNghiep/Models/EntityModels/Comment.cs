using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Comment
    {
        [Key]
        public Guid ID { get; set; }
        public string Content { get; set; }

        public Guid UserId { get; set; }
        public Account? Account { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid DiscussID { get; set; }
        public Discuss? Discuss { get; set; }
    }
}
