using MediatR;
using Modules.Shared.Domain.Events;
using Organization.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EventHandlers
{
    internal sealed class LeadershipPositionAssignedDomainEventHandler
     : INotificationHandler<LeadershipPositionAssignedDomainEvent>
    {
        private readonly IOrganizationUnitOfWork _uow;

        public LeadershipPositionAssignedDomainEventHandler(IOrganizationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(
            LeadershipPositionAssignedDomainEvent notification,
            CancellationToken cancellationToken)
        {
            var historyResult = LeadershipPositionHistory.Create(
                leadershipPositionId: notification.LeadershipPositionId,
                employeeId: notification.EmployeeId,
                startDate: notification.AssignedAt,
                notes: notification.Notes);

            if (historyResult.IsFailure)
                return;

            await _uow.LeadershipPositionHistoryRepository.AddAsync(historyResult.Value);
            await _uow.SaveChangesAsync();
        }
    }
}
