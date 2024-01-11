using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class AnswerCount
    {
        [Key]
        public Guid AnswerCountId { get; set; }
        public Guid OptionId { get; set; }
        public Option Option { get; set; }
    }
}
