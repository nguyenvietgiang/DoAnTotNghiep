using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Survey
    {
        [Key]
        public Guid SurveyId { get; set; }

        [Required]
        public string Title { get; set; }

        public List<Question> Questions { get; set; }
    }
}
