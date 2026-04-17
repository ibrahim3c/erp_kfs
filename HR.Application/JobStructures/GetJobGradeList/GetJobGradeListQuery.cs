using Modules.Shared.Application.Messaging;


namespace HR.Application.JobStructures.GetJobGradeList
{
    public record GetJobGradeListQuery() : IQuery<List<GetJobGradeListResponse>>;
}
