using HR.Application.Promotions.DTOs;
using Modules.Shared.Application.Messaging;


namespace HR.Application.Promotions.Queries
{
    public record GetPromotionHistoryQuery(Guid EmployeeId)
        : IQuery<EmployeePromotionHistoryResponse>;
}
