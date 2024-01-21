using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.SurveyRepo
{
    public class SurveyRepo : ISurveyRepo
    {
        private readonly DataContext _context; 

        public SurveyRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Survey>> GetAllSurveysAsync()
        {
            return await _context.Surveys.ToListAsync();
        }

        public async Task<Survey> GetSurveyByIdAsync(Guid surveyId)
        {
            return await _context.Surveys
                .Include(s => s.Questions)  // Include related Questions
                .FirstOrDefaultAsync(s => s.SurveyId == surveyId);
        }

        public async Task AddSurveyAsync(Survey survey)
        {
            _context.Surveys.Add(survey);
            await _context.SaveChangesAsync();
        }
    }

}
