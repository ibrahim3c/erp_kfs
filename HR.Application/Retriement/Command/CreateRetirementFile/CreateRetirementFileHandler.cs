using HR.Domain;
using HR.Domain.Retirement.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Command.CreateRetirementFile
{
    public class CreateRetirementFileHandler : ICommandHandler<CreateRetirementFileCommand, Guid>
    {
        private readonly IHRUnitOfWork _context;
        public CreateRetirementFileHandler(IHRUnitOfWork context) => _context = context;

        public async Task<Result<Guid>> Handle(CreateRetirementFileCommand request, CancellationToken cancellationToken)
        {
            var fileResult = RetirementFile.Create(request.EmployeeId, request.ReferralDate, request.Reason, request.ResponsibleEmployeeId);
            if (fileResult.IsFailure)
                return Result<Guid>.Failure(fileResult.Error);

            _context.RetriementRepository.Add(fileResult.Value);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(fileResult.Value.Id);
        }


    }
}
