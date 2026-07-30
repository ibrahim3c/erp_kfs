using HR.Application.Employees.GetAllEmployees;
using HR.Application.Legal.GetRulingList;
using HR.Application.Legal.GetRulingStats;

namespace ERP_KFS_MVC.Areas.HR.ViewModels
{
    public class RulingsPageViewModel
    {
        public GetRulingStatsResponse Stats { get; set; } = new();
        public List<GetRulingListResponse> Rulings { get; set; } = new();
        public IEnumerable<EmployeeListResponse> Employees { get; set; } = Enumerable.Empty<EmployeeListResponse>();
    }
}
