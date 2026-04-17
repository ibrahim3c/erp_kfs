using Modules.Shared.Domain;

namespace HR.Domain.Employees
{
    public sealed class EmployeeFinancial : Entity
    {
        public Guid EmployeeId { get; private set; }
        public decimal? BasicSalary2019 { get; private set; }
        public decimal? GrossSalary { get; private set; }
        public string InsuranceNumber { get; private set; }
        public string BankName { get; private set; }
        public string BankAccount { get; private set; }
        public bool HasFellowshipFund { get; private set; } // صندوق الزمالة
        public bool HasMedicalFund { get; private set; }    // صندوق التكافل

        private EmployeeFinancial() { } // For EF Core

        private EmployeeFinancial(Guid id, Guid employeeId, decimal? basicSalary2019,
            decimal? grossSalary, string insuranceNumber, string bankName,
            string bankAccount, bool hasFellowshipFund, bool hasMedicalFund) : base(id)
        {
            EmployeeId = employeeId;
            BasicSalary2019 = basicSalary2019;
            GrossSalary = grossSalary;
            InsuranceNumber = insuranceNumber;
            BankName = bankName;
            BankAccount = bankAccount;
            HasFellowshipFund = hasFellowshipFund;
            HasMedicalFund = hasMedicalFund;
        }

        public static Result<EmployeeFinancial> Create(
            Guid employeeId,
            decimal? basicSalary2019,
            decimal? grossSalary,
            string insuranceNumber,
            string bankName,
            string bankAccount,
            bool hasFellowshipFund,
            bool hasMedicalFund)
        {
            if (employeeId == Guid.Empty)
                return Result<EmployeeFinancial>.Failure(new Error("EmployeeFinancial.EmployeeIdEmpty", "يجب ربط البيانات المالية بموظف"));

            if (grossSalary.HasValue && grossSalary.Value < 0)
                return Result<EmployeeFinancial>.Failure(new Error("EmployeeFinancial.InvalidSalary", "الراتب لا يمكن أن يكون قيمة سالبة"));

            var financialInfo = new EmployeeFinancial(
                Guid.NewGuid(),
                employeeId,
                basicSalary2019,
                grossSalary,
                insuranceNumber,
                bankName,
                bankAccount,
                hasFellowshipFund,
                hasMedicalFund);

            return Result<EmployeeFinancial>.Success(financialInfo);
        }

        // Business Behavior
        public void Update(decimal? basic, decimal? gross, string insuranceNum, string bankName, string bankAccount, bool fellowship, bool medical)
        {
            BasicSalary2019 = basic;
            GrossSalary = gross;
            InsuranceNumber = insuranceNum;
            BankName = bankName;
            BankAccount = bankAccount;
            HasFellowshipFund = fellowship;
            HasMedicalFund = medical;
        }
    }
}