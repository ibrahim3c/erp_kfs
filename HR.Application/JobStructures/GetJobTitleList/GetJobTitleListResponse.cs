

namespace HR.Application.JobStructures.GetJobTitleList
{
    public class GetJobTitleListResponse
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Guid FunctionalGroupId { get; init; }
        public string FunctionalGroupName { get; init; } = string.Empty;
        public string QualitativeGroupName { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}
