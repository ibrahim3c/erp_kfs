using Modules.Shared.Domain;
namespace HR.Domain.Candidates
{
    public class NominationFile : Entity
    {
        public Guid CandidateId { get; private set; }
        public string FilePath { get; private set; }
        public DateTime ReceiveDate { get; private set; }
        public DateTime? ExpectedEndDate { get; private set; }
        public NominationStatus Status { get; private set; }
        public string ReferenceNumber { get; private set; }

        // 1. Parameterless constructor for EF Core
        private NominationFile() { }

        // 2. Internal constructor: Only Candidate can create a NominationFile
        private NominationFile(Guid id,Guid candidateId, string filePath, string referenceNumber, DateTime? expectedEndDate = null):base(id)
        {
            CandidateId = candidateId;
            FilePath = filePath;
            ReferenceNumber = referenceNumber;
            ReceiveDate = DateTime.UtcNow;
            ExpectedEndDate = expectedEndDate;
            Status = NominationStatus.Received; // Initial State
        }

        public static Result< NominationFile> Create(Guid candidateId, string filePath, string referenceNumber, DateTime? expectedEndDate = null)
        {
            // You can add domain validations here before creating the NominationFile
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path is required.", nameof(filePath));
            if (string.IsNullOrWhiteSpace(referenceNumber))
                throw new ArgumentException("Reference number is required.", nameof(referenceNumber));
            return Result<NominationFile>.Success(new NominationFile(Guid.NewGuid(), candidateId, filePath, referenceNumber, expectedEndDate));
        }

        // 3. Business Behaviors
        public void MarkAsUnderReview()
        {
            if (Status != NominationStatus.Received)
                throw new InvalidOperationException("لا يمكن مراجعة ملف لم يتم استلامه.");

            Status = NominationStatus.UnderReview;
        }

        public void Accept()
        {
            Status = NominationStatus.Accepted;
        }

        public void Reject()
        {
            Status = NominationStatus.Rejected;
        }
    }
}
