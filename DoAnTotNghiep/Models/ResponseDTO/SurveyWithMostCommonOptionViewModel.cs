using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Models.ResponseDTO
{
    public class SurveyWithMostCommonOptionViewModel
    {
        public Survey Survey { get; set; }
        public QuestionWithMostCommonOptionViewModel QuestionWithMostCommonOption { get; set; }
    }

    public class QuestionWithMostCommonOptionViewModel
    {
        public Question Question { get; set; }
        public Option MostCommonOption { get; set; }
    }
}
