using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.SurveyRepo
{
    public class OptionRepo : ISurveyRepo<Option>
    {
        private readonly DataContext _context;

        public OptionRepo(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<Option> GetAll()
        {
            return _context.Options.ToList();
        }

        public Option GetById(Guid id)
        {
            return _context.Options.Find(id);
        }

        public void Insert(Option entity)
        {
            _context.Options.Add(entity);
        }

        public void Update(Option entity)
        {
            _context.Options.Update(entity);
        }

        public void Delete(Option entity)
        {
            _context.Options.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }

}
