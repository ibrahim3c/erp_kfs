namespace HR.Domain.Funds
{
    public interface IFundRepository
    {
        Task<FundSubscription?> GetSubscriptionByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<FundSubscription>> GetAllSubscriptionsAsync(CancellationToken ct = default);
        Task<FundSubscription?> GetActiveSubscriptionByEmployeeAsync(Guid employeeId, FundType fundType, CancellationToken ct = default);
        void AddSubscription(FundSubscription subscription);
        void UpdateSubscription(FundSubscription subscription);

        Task<FundClaim?> GetClaimByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<FundClaim>> GetAllClaimsAsync(CancellationToken ct = default);
        void AddClaim(FundClaim claim);
        void UpdateClaim(FundClaim claim);
    }
}
