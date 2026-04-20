namespace Identity.Domain.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string HR = "HR";
        public const string Recruiter = "Recruiter";
        public const string Employee = "Employee";

        public static readonly string[] AllRoles =
        {
            Admin,
            HR,
            Recruiter,
            Employee
        };
    }
}
