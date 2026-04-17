using Modules.Shared.Application.Messaging;


namespace HR.Application.JobStructures.GetQualitativeGroupList
{
    public record GetQualitativeGroupListQuery() : IQuery<List<GetQualitativeGroupListResponse>>;
}
