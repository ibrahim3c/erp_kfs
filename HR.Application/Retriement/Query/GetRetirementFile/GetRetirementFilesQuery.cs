using MediatR;
using Modules.Shared.Application.Messaging;


namespace HR.Application.Retriement.Query.GetRetirementFile
{
    public record GetRetirementFilesQuery :IQuery<RetirementFilesResult>;
}
