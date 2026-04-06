using Modules.Shared.Domain;
namespace HR.Domain.Employees
{
    public class LeadershipPositionHistory : Entity
    {
        public Guid LeadershipPositionId { get; private set; }
        public Guid EmployeeId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public string DecisionNumber { get; private set; }
        public DateTime? DecisionDate { get; private set; }
        public string Notes { get; private set; }

        private LeadershipPositionHistory() { }

        // Internal: Only the Employee or HR Aggregate should create this
        internal LeadershipPositionHistory(Guid employeeId, Guid leadershipPositionId, DateTime startDate, string decisionNumber, DateTime? decisionDate, string notes)
        {
            EmployeeId = employeeId;
            LeadershipPositionId = leadershipPositionId;
            StartDate = startDate;
            DecisionNumber = decisionNumber;
            DecisionDate = decisionDate;
            Notes = notes;
        }

        // Behavior: إنهاء فترة المنصب
        public void EndPosition(DateTime endDate)
        {
            if (endDate < StartDate)
                throw new ArgumentException("تاريخ الانتهاء لا يمكن أن يكون قبل تاريخ البدء.");

            EndDate = endDate;
        }
    }
}
