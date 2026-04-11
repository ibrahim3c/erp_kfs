using Modules.Shared.Domain;

namespace Organization.Domain
{
    public sealed class LeadershipPosition : Entity
    {

        private LeadershipPosition() { }

        private LeadershipPosition(
            Guid id,
            Guid orgUnitId,
            Guid jobTitleId,
            string description,
            bool isActive) : base(id)
        {
            OrgUnitId = orgUnitId;
            JobTitleId = jobTitleId;
            Description = description;
            IsActive = isActive;
        }

        public Guid OrgUnitId { get; private set; }
        public Guid JobTitleId { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        //private readonly List<LeadershipPositionHistory> _histories = new();
        //public IReadOnlyCollection<LeadershipPositionHistory> LeadershipPositionHistories => _histories.AsReadOnly();

        // Factory
        public static Result<LeadershipPosition> Create(
            Guid orgUnitId,
            Guid jobTitleId,
            string description)
        {
            if (orgUnitId == Guid.Empty)
                return Result<LeadershipPosition>.Failure(OrganizationErrors.OrgUnitIdEmpty);

            if (jobTitleId == Guid.Empty)
                return Result<LeadershipPosition>.Failure(OrganizationErrors.JobTitleIdEmpty);

            var position = new LeadershipPosition(
                Guid.NewGuid(),
                orgUnitId,
                jobTitleId,
                description,
                true
            );

            return Result<LeadershipPosition>.Success(position);
        }

        // Business behaviors
        public Result Deactivate()
        {
            if (!IsActive)
                return Result.Failure(OrganizationErrors.LeadershipAlreadyInactive);

            IsActive = false;
            return Result.Success();
        }

        public Result Activate()
        {
            if (IsActive)
                return Result.Failure(OrganizationErrors.LeadershipAlreadyActive);

            IsActive = true;
            return Result.Success();
        }

        //public Result AddHistory(LeadershipPositionHistory history)
        //{
        //    if (history == null)
        //        return Result.Failure(new Error("LeadershipHistory.Null", "التاريخ لا يمكن أن يكون فارغاً"));

        //    _histories.Add(history);
        //    return Result.Success();
        //}
    }

}

