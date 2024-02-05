using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.SurveyRepo
{
    public class SurveyRepo : ISurveyRepo<Survey>
    {
        private readonly DataContext _context; 

        public SurveyRepo(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<Survey> GetAll()
        {
            return _context.Surveys.ToList();
        }

        public Survey GetById(Guid id)
        {
            return _context.Surveys.Find(id);
        }

        public void Insert(Survey entity)
        {
            _context.Surveys.Add(entity);
        }

        public void Update(Survey entity)
        {
            _context.Surveys.Update(entity);
        }

        public void Delete(Survey entity)
        {
            _context.Surveys.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }

}
