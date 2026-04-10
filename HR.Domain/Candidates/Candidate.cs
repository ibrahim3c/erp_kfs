using Modules.Shared.Domain;
using System.Text.RegularExpressions;
namespace HR.Domain.Candidates
{
    public class Candidate : Entity
    {
       
        public string FullName { get; private set; }
        public string NationalId { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public Guid QualificationTypeId { get; private set; }
        public Guid CityCenterId { get; private set; }
        public Guid VillageId { get; private set; }
        public bool IsActive { get; private set; }

        //public int? CreatedBy { get; set; }
        //public int? UpdatedBy { get; set; }
        //public int? DeletedBy { get; set; }


        // Encapsulated Collection
        private readonly List<NominationFile> _nominationFiles = new();
        public IReadOnlyCollection<NominationFile> NominationFiles => _nominationFiles.AsReadOnly();

        // 1. Parameterless constructor for EF Core
        private Candidate() { }

        // 2. Public Constructor for Creation
        private Candidate(Guid id,string fullName, string nationalId, string phone, string email,
                         Guid qualificationTypeId, Guid cityCenterId, Guid villageId):base(id)
        {
            FullName = fullName;
            NationalId = nationalId;
            Phone = phone;
            Email = email ?? string.Empty;
            QualificationTypeId = qualificationTypeId;
            CityCenterId = cityCenterId;
            VillageId = villageId;
            IsActive = true;
        }

        public static Result<Candidate> Create(string fullName, string nationalId, string phone, string email,
                                            Guid qualificationTypeId, Guid cityCenterId, Guid villageId)
        {
            // You can add domain validations here before creating the Candidate
             if (string.IsNullOrWhiteSpace(fullName))
                 return Result<Candidate>.Failure(CandidateErrors.FullNameEmpty);

            if (string.IsNullOrWhiteSpace(nationalId))
                 return Result<Candidate>.Failure(CandidateErrors.NationalIdEmpty);

            if (string.IsNullOrWhiteSpace(phone))
                 return Result<Candidate>.Failure(CandidateErrors.PhoneEmpty);

            if (qualificationTypeId == Guid.Empty)
                    return Result<Candidate>.Failure(CandidateErrors.QualificationRequired);

            if (cityCenterId == Guid.Empty)
                    return Result<Candidate>.Failure(CandidateErrors.CityCenterRequired);

            if (villageId == Guid.Empty)
                    return Result<Candidate>.Failure(CandidateErrors.VillageRequired);

            if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
                return Result<Candidate>.Failure(CandidateErrors.EmailInvalid);

            var candidate = new Candidate(Guid.NewGuid(), fullName, nationalId, phone, email, qualificationTypeId, cityCenterId, villageId);

            return Result<Candidate>.Success(candidate);
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        // 3. Business Behaviors
        public void UpdateContactInfo(string phone, string email)
        {
            Phone = phone;
            Email = email;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    

        // 4. Managing Child Entities (Nomination File)
        public Result AddNominationFile(string filePath, string referenceNumber, DateTime? expectedEndDate = null)
        {
            // The Aggregate Root creates the child entity and passes its own Id
            var file = NominationFile.Create(Id, filePath, referenceNumber, expectedEndDate);
            if(file.IsFailure)
                return Result.Failure(file.Error); // Propagate the error if creation failed

            if (_nominationFiles.Any(f => f.FilePath == filePath))
                return Result.Failure(CandidateErrors.DuplicateNominationFile);

            _nominationFiles.Add(file.Value!);
            return Result.Success();
        }
    }
}
