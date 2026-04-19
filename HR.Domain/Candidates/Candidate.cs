using Modules.Shared.Domain;
namespace HR.Domain.Candidates
{
    public class Candidate : Entity
    {
       
        public string FullName { get; private set; }
        public string NationalId { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        //public Guid QualificationTypeId { get; private set; }
        public Guid? CityCenterId { get; private set; }
        public Guid? VillageId { get; private set; }
        public bool IsActive { get; private set; }

        // Encapsulated Collection
        private readonly List<NominationFile> _nominationFiles = new();
        public IReadOnlyCollection<NominationFile> NominationFiles => _nominationFiles.AsReadOnly();

        // 1. Parameterless constructor for EF Core
        private Candidate() { }

        private Candidate(Guid id,string fullName, string nationalId, string phone, string email,
                         Guid cityCenterId, Guid villageId):base(id)
        {
            FullName = fullName;
            NationalId = nationalId;
            Phone = phone;
            Email = email ?? string.Empty;
            //QualificationTypeId = qualificationTypeId;
            CityCenterId = cityCenterId;
            VillageId = villageId;
            IsActive = true;
        }

            public static Result<Candidate> Create(string fullName, string nationalId, string phone, string email,
                                           Guid cityCenterId, Guid villageId)
            {
                // You can add domain validations here before creating the Candidate
                if (string.IsNullOrWhiteSpace(fullName))
                    throw new ArgumentException("Full Name is required.", nameof(fullName));
    
                if (string.IsNullOrWhiteSpace(nationalId))
                    throw new ArgumentException("National ID is required.", nameof(nationalId));
    
                return Result<Candidate>.Success(new Candidate(Guid.NewGuid(),fullName, nationalId, phone, email, cityCenterId, villageId));
        }   

        public void UpdateContactInfo(string phone, string email)
        {
            Phone = phone;
            Email = email;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public Result AddNominationFile(string filePath, string referenceNumber, DateTime? expectedEndDate = null)
        {
            // The Aggregate Root creates the child entity and passes its own Id
            var file = NominationFile.Create(Id, filePath, referenceNumber, expectedEndDate);
            if(file.IsFailure)
                return Result.Failure(file.Error); // Propagate the error if creation failed
            _nominationFiles.Add(file.Value);
            return Result.Success();
        }
    }
}
