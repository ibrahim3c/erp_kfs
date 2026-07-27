namespace HR.Application.Leaves.GetLeaveBalance
{
    public class GetLeaveBalanceResponse
    {
        public int RegularLeaveEntitled { get; init; }
        public int RegularLeaveUsed { get; init; }
        public int RegularRemaining { get; init; }
        public int CasualLeaveEntitled { get; init; }
        public int CasualLeaveUsed { get; init; }
        public int CasualRemaining { get; init; }
        public int CarryOverRegularDays { get; init; }
    }
}
