using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.PolicyRepo
{
    public interface IPolicyRepository
    {
        Task<IEnumerable<Policy>> GetAllPoliciesAsync();
        Task<Policy> GetPolicyByIdAsync(Guid policyId);
        Task AddPolicyAsync(Policy policy);
        Task UpdatePolicyAsync(Policy policy);
        Task DeletePolicyAsync(Guid policyId);
    }
}
