namespace HR.Domain.Legal
{
    public interface ICourtRulingRepository
    {
        Task<CourtRuling?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<CourtRuling>> GetAllAsync(CancellationToken ct = default);
        void Add(CourtRuling ruling);
        void Update(CourtRuling ruling);
    }
}
