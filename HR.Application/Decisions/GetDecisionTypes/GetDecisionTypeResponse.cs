namespace HR.Application.Decisions.GetDecisionTypes
{
    public class GetDecisionTypeResponse
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
