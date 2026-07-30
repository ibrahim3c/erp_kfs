using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Query.GetExternalMovements
{
    public record ExternalMovementListItemDto(
      Guid Id, string EmployeeName, string Type, string Direction, string OtherEntityName,
      DateTime? StartDate, DateTime? EndDate, string Status, string? AttachmentPath);

}
