using Modules.Shared.Domain;

namespace HR.Domain.Employees
{
    public class AcademicIncentiveRequest : Entity
    {
        public Guid EmployeeId { get; private set; }
        public Guid AcademicIncentiveTypeId { get; private set; }
        public Guid QualificationId { get; private set; }
        public DateTime RequestDate { get; private set; }
        public AcademicIncentiveStatus Status { get; private set; }
        public DateTime? RequestAffectDate { get; private set; }
        public string Notes { get; private set; }
        public string FilePath { get; private set; }


        //public DateTime CreatedAt { get; set; }
        //public int? CreatedBy { get; set; }

        //public DateTime? UpdatedAt { get; set; }
        //public int? UpdatedBy { get; set; }

        //public DateTime? DeletedAt { get; set; }
        //public int? DeletedBy { get; set; }

        private AcademicIncentiveRequest() { }

        // Constructor for a new request
        public AcademicIncentiveRequest(Guid employeeId, Guid typeId, Guid qualificationId, string filePath, string notes = "")
        {
            EmployeeId = employeeId;
            AcademicIncentiveTypeId = typeId;
            QualificationId = qualificationId;
            FilePath = filePath;
            Notes = notes;
            RequestDate = DateTime.UtcNow;
            Status = AcademicIncentiveStatus.Draft; // بيبدأ كمسودة
        }

        // --- Workflow Methods ---
        public void Submit()
        {
            if (Status != AcademicIncentiveStatus.Draft)
                throw new InvalidOperationException("لا يمكن تقديم طلب ليس في حالة المسودة.");

            Status = AcademicIncentiveStatus.Submitted;
        }

        public void Approve(DateTime affectDate)
        {
            Status = AcademicIncentiveStatus.Approved;
            RequestAffectDate = affectDate;
        }

        public void Reject(string rejectionReason)
        {
            Status = AcademicIncentiveStatus.Rejected;
            Notes = rejectionReason;
        }
    }
}
