using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Survey
    {
        [Key]
        public Guid SurveyId { get; set; }

        [Required]
        public string Title { get; set; }

        public bool Status { get; set; } = true;
        public SurveyTarget surveyTarget  { get; set; }

        public List<Question> Questions { get; set; }
    }

public enum SurveyTarget
{
    Candidate = 1,
    Employer = 2
}
}
