using DoAnTotNghiep.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Repository.CandidatesRepo
{
    public class CandidatesRepocs : ICandidatesRepo
    {
        private readonly DataContext _context;

        public CandidatesRepocs(DataContext context)
        {
            _context = context;
        }
        public async Task<Candidate> GetCandidateByIdAsync(Guid candidateId)
        {
            return await _context.Candidates
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.CandidateID == candidateId);
        }

        public async Task UpdateCandidateAsync(Candidate candidate)
        {
            _context.Entry(candidate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
