using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.SurveyRepo
{
    public class QuestionRepo : ISurveyRepo<Question>
    {
        private readonly DataContext _context;

        public QuestionRepo(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<Question> GetAll()
        {
            return _context.Questions.ToList();
        }

        public Question GetById(Guid id)
        {
            return _context.Questions.Find(id);
        }

        public void Insert(Question entity)
        {
            _context.Questions.Add(entity);
        }

        public void Update(Question entity)
        {
            _context.Questions.Update(entity);
        }

        public void Delete(Question entity)
        {
            _context.Questions.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }

}
