namespace Identity.Domain.Constants
{
    public static class Permissions
    {
        // ===== Leadership =====
        public const string LeadershipManage = "Leadership.Manage";

        // ===== Admin =====
        public const string AdminManage = "Admin.Manage";
        public const string UsersManage = "Users.Manage";
        public const string RolesManage = "Roles.Manage";
        public const string PermissionsManage = "Permissions.Manage";
        public const string DashboardView = "Dashboard.View";

        // ===== HR - Employees =====
        public const string EmployeesView = "Employees.View";
        public const string EmployeesCreate = "Employees.Create";
        public const string EmployeesEdit = "Employees.Edit";
        public const string EmployeesDelete = "Employees.Delete";
        public const string EmployeesSearch = "Employees.Search";

        // ===== HR - Lifecycle =====
        public const string PromotionsManage = "Promotions.Manage";
        public const string TransfersManage = "Transfers.Manage";
        public const string SecondmentsManage = "Secondments.Manage";
        public const string TerminationsManage = "Terminations.Manage";
        public const string RetirementManage = "Retirement.Manage";
        public const string LeavesManage = "Leaves.Manage";
        public const string AttendanceManage = "Attendance.Manage";
        public const string AbsenceManage = "Absence.Manage";

        // ===== HR - Benefits =====
        public const string LoansManage = "Loans.Manage";
        public const string PayrollView = "Payroll.View";
        public const string PayrollManage = "Payroll.Manage";
        public const string FundsManage = "Funds.Manage";
        public const string EvaluationManage = "Evaluation.Manage";

        // ===== HR - Other =====
        public const string DecisionsManage = "Decisions.Manage";
        public const string PenaltiesManage = "Penalties.Manage";
        public const string LegalReview = "Legal.Review";
        public const string LookupsManage = "Lookups.Manage";

        // ===== Other Areas =====
        public const string ProjectsView = "Projects.View";
        public const string InventoryView = "Inventory.View";
        public const string FleetView = "Fleet.View";
        public const string EducationView = "Education.View";
        public const string AccountingView = "Accounting.View";

        public static readonly string[] AllPermissions =
        {
            LeadershipManage,
            AdminManage,
            UsersManage,
            RolesManage,
            PermissionsManage,
            DashboardView,
            EmployeesView,
            EmployeesCreate,
            EmployeesEdit,
            EmployeesDelete,
            EmployeesSearch,
            PromotionsManage,
            TransfersManage,
            SecondmentsManage,
            TerminationsManage,
            RetirementManage,
            LeavesManage,
            AttendanceManage,
            AbsenceManage,
            LoansManage,
            PayrollView,
            PayrollManage,
            FundsManage,
            EvaluationManage,
            DecisionsManage,
            PenaltiesManage,
            LegalReview,
            LookupsManage,
            ProjectsView,
            InventoryView,
            FleetView,
            EducationView,
            AccountingView
        };
    }
}