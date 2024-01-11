using Microsoft.CodeAnalysis.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Question
    {
        [Key]
        public Guid QuestionId { get; set; }

        [Required]
        public string QuestionText { get; set; }

        public Guid SurveyId { get; set; }
        public Survey Survey { get; set; }

        public List<Option> Options { get; set; }
    }
}
