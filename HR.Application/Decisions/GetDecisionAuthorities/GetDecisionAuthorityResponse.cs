namespace HR.Application.Decisions.GetDecisionAuthorities
{
    public class GetDecisionAuthorityResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
