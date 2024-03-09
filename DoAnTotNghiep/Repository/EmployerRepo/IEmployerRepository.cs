using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.ResponseDTO;

namespace DoAnTotNghiep.Repository.EmployerRepo
{ 
    public interface IEmployerRepository
    {
        Task<IEnumerable<Employer>> GetAllAsync();
        Task<Employer> GetEmployerByIdAsync(Guid employerId);
        Task<List<Employer>> GetTop5EmployersWithJobCount();

        Task<List<EmployerJobPostCount>> GetTop3EmployersWithJobPostCounts();
    }
}
