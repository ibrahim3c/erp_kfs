using HR.Domain.Secondments.Enums;
using Microsoft.AspNetCore.Http;
using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Command
{
    public record CreateSecondmentCommand(
     Guid EmployeeId, SecondmentType Type, string HostEntityName,
     DateTime StartDate, DateTime EndDate,
     SalaryBearer SalaryBearer, IncentiveBearer IncentiveBearer, IFormFile? File) : ICommand<Guid>;
}
