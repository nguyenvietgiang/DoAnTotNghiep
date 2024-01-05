using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.CandidatesRepo
{
    public interface ICandidatesRepo
    {
        Task<IEnumerable<Candidate>> GetAllAsync();
        Task<Candidate> GetCandidateByIdAsync(Guid candidateId);
        Task UpdateCandidateAsync(Candidate candidate);
    }
}
