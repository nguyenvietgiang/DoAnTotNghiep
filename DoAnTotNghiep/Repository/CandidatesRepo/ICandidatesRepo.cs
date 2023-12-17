using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Repository.CandidatesRepo
{
    public interface ICandidatesRepo
    {
        Task<Candidate> GetCandidateByIdAsync(Guid candidateId);
        Task UpdateCandidateAsync(Candidate candidate);
    }
}
