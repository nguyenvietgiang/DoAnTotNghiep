using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.SurveyRepo
{
    public interface ISurveyRepo<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
