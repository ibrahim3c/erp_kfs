using HR.Domain.JobStructures;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class JobStructureRepository : IJobStructureRepository
    {
        private readonly HRDbContext dbContext;

        public JobStructureRepository(HRDbContext _dbContext)   
        {
            this.dbContext = _dbContext;
        }

        public void AddJobGrade(JobGrade jobGrade)
        {
            dbContext.JobGrades.Add(jobGrade);
        }

        public void AddJobTitle(JobTitle jobTitle)
        {
            dbContext.JobTitles.Add(jobTitle);
        }

        public async Task<JobTitle> GetJobTitleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await dbContext.JobTitles.Include(j => j.FunctionalGroup).FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
        }
    }
}
