using HR.Domain.Terminations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.Terminations
{
    public class TerminationDecisionConfiguration : IEntityTypeConfiguration<TerminationDecision>
    {
        public void Configure(EntityTypeBuilder<TerminationDecision> builder)
        {
            builder.ToTable("TerminationDecisions", Schemas.HR);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DecisionNumber).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Reason).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.LegalBasis).HasMaxLength(1000);
            builder.Property(x => x.AttachmentPath).HasMaxLength(500);
            builder.Property(x => x.CancellationReason).HasMaxLength(500);

            builder.HasIndex(x => x.EmployeeId);
            builder.HasIndex(x => x.DecisionNumber).IsUnique();
        }
    }
}
