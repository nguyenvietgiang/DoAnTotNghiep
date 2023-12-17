using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Response
    {
        [Key]
        public Guid ResponseId { get; set; }

        public Guid UserID { get; set; }

        public Guid OptionId { get; set; }

        [ForeignKey("OptionId")]
        public Option? Option { get; set; }
    }
}
