using Modules.Shared.Domain;
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

        // Encapsulated Collection
        private readonly List<NominationFile> _nominationFiles = new();
        public IReadOnlyCollection<NominationFile> NominationFiles => _nominationFiles.AsReadOnly();

        // 1. Parameterless constructor for EF Core
        private Candidate() { }

        // 2. Public Constructor for Creation
        public Candidate(string fullName, string nationalId, string phone, string email,
                         Guid qualificationTypeId, Guid cityCenterId, Guid villageId)
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

        // 3. Business Behaviors
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

        // 4. Managing Child Entities (Nomination File)
        public void AddNominationFile(string filePath, string referenceNumber, DateTime? expectedEndDate = null)
        {
            // The Aggregate Root creates the child entity and passes its own Id
            var file = new NominationFile(Id, filePath, referenceNumber, expectedEndDate);
            _nominationFiles.Add(file);
        }
    }
}
