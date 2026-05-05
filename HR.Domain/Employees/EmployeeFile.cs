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
            string personalPhoto,
            string contractFile) : base(id)
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
            ContractFile = contractFile;
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
        public string ContractFile { get; private set; }

        public static Result<EmployeeFile> Create(
            Guid employeeId,
            string militaryFile,
            string qualificationFile,
            string birthCertificateFile,
            string policeClearanceCertificate,
            string nationalIdCardFront,
            string nationalIdCardBack,
            string marriageDocument,
            string personalPhoto,
            string contractFile)
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
                personalPhoto,
                contractFile))
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
                personalPhoto,
                contractFile
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
        public Result UpdateFiles(
    string? personalPhoto,
    string? nationalIdCardFront,
    string? nationalIdCardBack,
    string? qualificationFile,
    string? birthCertificateFile,
    string? militaryFile,
    string? contractFile,
    string? policeClearanceCertificate,
    string? marriageDocument)
        {
            if (!string.IsNullOrWhiteSpace(personalPhoto))
                PersonalPhoto = personalPhoto;

            if (!string.IsNullOrWhiteSpace(nationalIdCardFront))
                NationalIdCardFront = nationalIdCardFront;

            if (!string.IsNullOrWhiteSpace(nationalIdCardBack))
                NationalIdCardBack = nationalIdCardBack;

            if (!string.IsNullOrWhiteSpace(qualificationFile))
                QualificationFile = qualificationFile;

            if (!string.IsNullOrWhiteSpace(birthCertificateFile))
                BirthCertificateFile = birthCertificateFile;

            if (!string.IsNullOrWhiteSpace(militaryFile))
                MilitaryFile = militaryFile;

            if (!string.IsNullOrWhiteSpace(contractFile))
                ContractFile = contractFile;

            if (!string.IsNullOrWhiteSpace(policeClearanceCertificate))
                PoliceClearanceCertificate = policeClearanceCertificate;

            if (!string.IsNullOrWhiteSpace(marriageDocument))
                MarriageDocument = marriageDocument;

            return Result.Success();
        }

        private static bool AllFilesEmpty(
            string militaryFile,
            string qualificationFile,
            string birthCertificateFile,
            string policeClearanceCertificate,
            string nationalIdCardFront,
            string nationalIdCardBack,
            string marriageDocument,
            string personalPhoto,
            string contractFile)
        {
            return string.IsNullOrWhiteSpace(militaryFile) &&
                   string.IsNullOrWhiteSpace(qualificationFile) &&
                   string.IsNullOrWhiteSpace(birthCertificateFile) &&
                   string.IsNullOrWhiteSpace(policeClearanceCertificate) &&
                   string.IsNullOrWhiteSpace(nationalIdCardFront) &&
                   string.IsNullOrWhiteSpace(nationalIdCardBack) &&
                   string.IsNullOrWhiteSpace(marriageDocument) &&
                   string.IsNullOrWhiteSpace(contractFile) &&
                   string.IsNullOrWhiteSpace(personalPhoto);
        }
    }
}
