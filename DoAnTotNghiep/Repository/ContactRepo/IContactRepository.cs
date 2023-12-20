using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.ContactRepo
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllAsync();

        Task<int> CountAsync();
        Task<Contact> GetByIdAsync(Guid id);
        Task CreateAsync(Contact entity);
        Task UpdateAsync(Contact entity);
        Task DeleteAsync(Guid id);
        Task ToggleStatusAsync(Guid id);
    }
}
