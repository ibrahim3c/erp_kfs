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
        internal NominationFile(Guid candidateId, string filePath, string referenceNumber, DateTime? expectedEndDate = null)
        {
            CandidateId = candidateId;
            FilePath = filePath;
            ReferenceNumber = referenceNumber;
            ReceiveDate = DateTime.UtcNow;
            ExpectedEndDate = expectedEndDate;
            Status = NominationStatus.Received; // Initial State
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
