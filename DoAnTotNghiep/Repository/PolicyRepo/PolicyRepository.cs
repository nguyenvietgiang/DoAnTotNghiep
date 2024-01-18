using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.PolicyRepo
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly DataContext _context;

        public PolicyRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
        {
            return await _context.Policies.ToListAsync();
        }

        public async Task<Policy> GetPolicyByIdAsync(Guid policyId)
        {
            return await _context.Policies.FirstOrDefaultAsync(p => p.PolicyID == policyId);
        }

        public async Task AddPolicyAsync(Policy policy)
        {
            _context.Policies.Add(policy);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePolicyAsync(Policy policy)
        {
            _context.Entry(policy).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePolicyAsync(Guid policyId)
        {
            var policy = await _context.Policies.FindAsync(policyId);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
                await _context.SaveChangesAsync();
            }
        }
    }
}
