using CollegeControlSystem.Domain.Abstractions;
using MediatR;
using Modules.Shared.Domain.Events;
using Organization.Application.IServices;
using Organization.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.EventHandlers
{
    public class LeadershipPositionRemoveDomainEventHandler : INotificationHandler<LeadershipPositionRemovedDomainEvent>
    {

        private readonly IOrganizationUnitOfWork unitOfWork;

        public LeadershipPositionRemoveDomainEventHandler(IOrganizationUnitOfWork unitOfWork)
        {

            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(LeadershipPositionRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var historyEntity = await unitOfWork.LeadershipPositionHistoryRepository
                .FindAsync(x => x.EmployeeId == notification.EmployeeId && x.EndDate == null);

            if (historyEntity != null)
            {

                var result = historyEntity.EndPosition(DateTime.UtcNow);

                if (result.IsSuccess)
                {
                    unitOfWork.LeadershipPositionHistoryRepository.Update(historyEntity);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}

