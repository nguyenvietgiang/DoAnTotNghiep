using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.EmployerRepo
{ 
    public interface IEmployerRepository
    {
        Task<IEnumerable<Employer>> GetAllAsync();
        Task<Employer> GetEmployerByIdAsync(Guid employerId);
        Task<List<Employer>> GetTop5EmployersWithJobCount();
    }
}
