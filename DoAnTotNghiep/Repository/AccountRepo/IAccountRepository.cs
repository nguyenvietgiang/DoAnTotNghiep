namespace DoAnTotNghiep.Repository.AccountRepo
{
    public interface IAccountRepository
    {
        Task UpdateAccountStatusAsync(Guid userId);
    }
}
