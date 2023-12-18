using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.EmployerRepo
{
    public interface IEmployerRepository
    {
        Task<Employer> GetEmployerByIdAsync(Guid employerId);
    }
}
