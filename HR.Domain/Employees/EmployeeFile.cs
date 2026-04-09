using Modules.Shared.Domain;

namespace HR.Domain.Employees
{
    public sealed class EmployeeFile : Entity
    {
        private EmployeeFile() { }

        private EmployeeFile(
            Guid id,
            Guid employeeId,
            string militaryFile,
            string qualificationFile,
            string birthCertificateFile,
            string policeClearanceCertificate,
            string nationalIdCardFront,
            string nationalIdCardBack,
            string marriageDocument,
            string personalPhoto) : base(id)
        {
            EmployeeId = employeeId;
            MilitaryFile = militaryFile;
            QualificationFile = qualificationFile;
            BirthCertificateFile = birthCertificateFile;
            PoliceClearanceCertificate = policeClearanceCertificate;
            NationalIdCardFront = nationalIdCardFront;
            NationalIdCardBack = nationalIdCardBack;
            MarriageDocument = marriageDocument;
            PersonalPhoto = personalPhoto;
        }

        public Guid EmployeeId { get; private set; }

        public string MilitaryFile { get; private set; }

        public string QualificationFile { get; private set; }

        public string BirthCertificateFile { get; private set; }

        public string PoliceClearanceCertificate { get; private set; }

        public string NationalIdCardFront { get; private set; }

        public string NationalIdCardBack { get; private set; }

        public string MarriageDocument { get; private set; }

        public string PersonalPhoto { get; private set; }


        public static Result<EmployeeFile> Create(
            Guid employeeId,
            string militaryFile,
            string qualificationFile,
            string birthCertificateFile,
            string policeClearanceCertificate,
            string nationalIdCardFront,
            string nationalIdCardBack,
            string marriageDocument,
            string personalPhoto)
        {
            if (employeeId == Guid.Empty)
                return Result<EmployeeFile>.Failure(EmployeeErrors.EmployeeIdEmpty);

            if (AllFilesEmpty(
                militaryFile,
                qualificationFile,
                birthCertificateFile,
                policeClearanceCertificate,
                nationalIdCardFront,
                nationalIdCardBack,
                marriageDocument,
                personalPhoto))
            {
                return Result<EmployeeFile>.Failure(EmployeeErrors.EmployeeFileRequired);
            }

            var employeeFile = new EmployeeFile(
                Guid.NewGuid(),
                employeeId,
                militaryFile,
                qualificationFile,
                birthCertificateFile,
                policeClearanceCertificate,
                nationalIdCardFront,
                nationalIdCardBack,
                marriageDocument,
                personalPhoto
            );

            return Result<EmployeeFile>.Success(employeeFile);
        }

        // Business Behaviors

        public Result UpdateMilitaryFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Result.Failure(EmployeeErrors.InvalidFilePath);

            MilitaryFile = filePath;

            return Result.Success();
        }

        public Result UpdateQualificationFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Result.Failure(EmployeeErrors.InvalidFilePath);

            QualificationFile = filePath;

            return Result.Success();
        }

        public Result UpdatePersonalPhoto(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Result.Failure(EmployeeErrors.InvalidFilePath);

            PersonalPhoto = filePath;

            return Result.Success();
        }

        // Helpers

        private static bool AllFilesEmpty(
            string militaryFile,
            string qualificationFile,
            string birthCertificateFile,
            string policeClearanceCertificate,
            string nationalIdCardFront,
            string nationalIdCardBack,
            string marriageDocument,
            string personalPhoto)
        {
            return string.IsNullOrWhiteSpace(militaryFile) &&
                   string.IsNullOrWhiteSpace(qualificationFile) &&
                   string.IsNullOrWhiteSpace(birthCertificateFile) &&
                   string.IsNullOrWhiteSpace(policeClearanceCertificate) &&
                   string.IsNullOrWhiteSpace(nationalIdCardFront) &&
                   string.IsNullOrWhiteSpace(nationalIdCardBack) &&
                   string.IsNullOrWhiteSpace(marriageDocument) &&
                   string.IsNullOrWhiteSpace(personalPhoto);
        }
    }
}
