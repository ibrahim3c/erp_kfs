using Modules.Shared.Application.Messaging;


namespace HR.Application.JobStructures.GetJobTitleList
{
    public record GetJobTitleListQuery() : IQuery<List<GetJobTitleListResponse>>;
}
