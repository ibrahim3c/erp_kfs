using Modules.Shared.Domain;

namespace HR.Domain.Employees
{
    //public enum EmploymentType
    //{
    //    Competition = 1, // تعيين بمسابقة
    //    TemporaryContract = 2, // تعاقد مؤقت
    //    BudgetBand = 3, // بند موازنة
    //    DailyWage = 4 // يومية/سركي
    //}
    public sealed class EmploymentType : Entity
    {
        private EmploymentType() { }

        private EmploymentType(Guid id, string code, string name, string description, bool isActive)
            : base(id)
        {
            Code = code;
            Name = name;
            Description = description;
            IsActive = isActive;
        }

        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public bool IsActive { get; private set; }

        public static EmploymentType Create(string code, string name, string description)
        {
            return new EmploymentType(
                Guid.NewGuid(),
                code,
                name,
                description,
                true
            );
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}