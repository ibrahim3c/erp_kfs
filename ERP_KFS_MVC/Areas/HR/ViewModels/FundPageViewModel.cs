using HR.Application.Employees.GetAllEmployees;
using HR.Application.Funds.GetFundClaims;
using HR.Application.Funds.GetFundStats;
using HR.Application.Funds.GetFundSubscriptions;

namespace ERP_KFS_MVC.Areas.HR.ViewModels
{
    public class FundPageViewModel
    {
        public GetFundStatsResponse Stats { get; set; } = new();
        public List<GetFundSubscriptionsResponse> Subscriptions { get; set; } = new();
        public List<GetFundClaimsResponse> Claims { get; set; } = new();
        public IEnumerable<EmployeeListResponse> Employees { get; set; } = Enumerable.Empty<EmployeeListResponse>();
    }
}
