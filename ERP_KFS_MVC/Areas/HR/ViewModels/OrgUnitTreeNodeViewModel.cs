namespace ERP_KFS_MVC.Areas.HR.ViewModels
{
    public class OrgUnitTreeNodeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int LevelOrder { get; set; }
        public string? CurrentManagerName { get; set; }
        public int EmployeeCount { get; set; }
        public List<OrgUnitTreeNodeViewModel> Children { get; set; } = new();
    }
}
