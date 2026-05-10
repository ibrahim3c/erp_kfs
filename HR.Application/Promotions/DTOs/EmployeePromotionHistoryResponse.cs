

namespace HR.Application.Promotions.DTOs
{
    public class EmployeePromotionHistoryResponse
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string CurrentGrade { get; set; } = string.Empty;
        public List<PromotionHistoryDto> Items { get; set; } = new();
    }
}
