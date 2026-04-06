namespace HR.Domain.Candidates
{
    public interface ICandidateRepository
    {
        Task<Candidate> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // (Eager Loading)
        Task<Candidate> GetByIdWithFilesAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Candidate> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default);

        void Add(Candidate candidate);
        void Update(Candidate candidate);
        void Delete(Candidate candidate);
    }
}
