using Modules.Shared.Domain;

namespace HR.Domain.Employees.Incentives
{
        public sealed class AcademicIncentiveRequest : Entity
        {
            private AcademicIncentiveRequest() { }

            private AcademicIncentiveRequest(
                Guid id,
                Guid employeeId,
                Guid academicIncentiveTypeId,
                Guid qualificationId,
                DateTime requestDate,
                DateTime? requestAffectDate,
                string notes,
                string filePath) : base(id)
            {
                EmployeeId = employeeId;
                AcademicIncentiveTypeId = academicIncentiveTypeId;
                QualificationId = qualificationId;
                RequestDate = requestDate;
                RequestAffectDate = requestAffectDate;
                Notes = notes;
                FilePath = filePath;
                Status = AcademicIncentiveStatus.Draft;
            }

            public Guid EmployeeId { get; private set; }

            public Guid AcademicIncentiveTypeId { get; private set; }

            public Guid QualificationId { get; private set; }

            public DateTime RequestDate { get; private set; }

            public AcademicIncentiveStatus Status { get; private set; }

            public DateTime? RequestAffectDate { get; private set; }

            public string Notes { get; private set; }

            public string FilePath { get; private set; }
            public static Result<AcademicIncentiveRequest> Create(
                Guid employeeId,
                Guid academicIncentiveTypeId,
                Guid qualificationId,
                DateTime requestDate,
                DateTime? requestAffectDate,
                string notes,
                string filePath)
            {
                if (employeeId == Guid.Empty)
                    return Result<AcademicIncentiveRequest>.Failure(EmployeeErrors.EmployeeIdEmpty);

                if (academicIncentiveTypeId == Guid.Empty)
                    return Result<AcademicIncentiveRequest>.Failure(EmployeeErrors.AcademicIncentiveTypeIdEmpty);

                if (qualificationId == Guid.Empty)
                    return Result<AcademicIncentiveRequest>.Failure(EmployeeErrors.QualificationIdEmpty);

                if (requestAffectDate.HasValue && requestAffectDate < requestDate)
                    return Result<AcademicIncentiveRequest>.Failure(EmployeeErrors.InvalidAffectDate);

                var request = new AcademicIncentiveRequest(
                    Guid.NewGuid(),
                    employeeId,
                    academicIncentiveTypeId,
                    qualificationId,
                    requestDate,
                    requestAffectDate,
                    notes,
                    filePath
                );

                return Result<AcademicIncentiveRequest>.Success(request);
            }

            // Business Behaviors

            public Result Submit()
            {
                if (Status != AcademicIncentiveStatus.Draft)
                    return Result.Failure(EmployeeErrors.RequestAlreadySubmitted);

                Status = AcademicIncentiveStatus.Submitted;

                return Result.Success();
            }

            public Result Approve()
            {
                if (Status == AcademicIncentiveStatus.Approved)
                    return Result.Failure(EmployeeErrors.RequestAlreadyApproved);

                Status = AcademicIncentiveStatus.Approved;

                return Result.Success();
            }

            public Result Reject(string reason)
            {
                if (Status == AcademicIncentiveStatus.Rejected)
                    return Result.Failure(EmployeeErrors.RequestAlreadyRejected);

                Status = AcademicIncentiveStatus.Rejected;
                Notes = reason;

                return Result.Success();
            }
        }
    }