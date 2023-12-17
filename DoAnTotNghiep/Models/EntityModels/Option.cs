using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Option
    {
        [Key]
        public Guid OptionId { get; set; }

        [Required]
        public string OptionText { get; set; }
        public Guid QuestionId { get; set; }

        public Question? Question
        {
            get; set;
        }
        public List<Response> Responses { get; set; }
    }
}
