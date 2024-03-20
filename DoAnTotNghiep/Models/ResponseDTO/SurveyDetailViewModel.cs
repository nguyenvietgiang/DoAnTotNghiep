using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Models.ResponseDTO
{
    public class SurveyDetailViewModel
    {
        public Survey Survey { get; set; }
        public List<QuestionWithOptionCountsViewModel> QuestionsWithOptionCounts { get; set; }
    }
}
