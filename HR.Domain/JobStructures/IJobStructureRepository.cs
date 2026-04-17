namespace HR.Domain.JobStructures
{
    public interface IJobStructureRepository
    {
        void AddJobTitle(JobTitle jobTitle);
        void AddJobGrade(JobGrade jobGrade);
        Task<JobTitle> GetJobTitleByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
