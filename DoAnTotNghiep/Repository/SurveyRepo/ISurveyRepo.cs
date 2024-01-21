using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.SurveyRepo
{
    public interface ISurveyRepo
    {
        Task<IEnumerable<Survey>> GetAllSurveysAsync();
        Task<Survey> GetSurveyByIdAsync(Guid surveyId);
        Task AddSurveyAsync(Survey survey);
    }
}
