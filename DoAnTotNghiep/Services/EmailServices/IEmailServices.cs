namespace DoAnTotNghiep.Services.EmailServices
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string email, string content);
    }
}
