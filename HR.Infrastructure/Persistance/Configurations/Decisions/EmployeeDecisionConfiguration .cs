using HR.Domain.Decisions;
using HR.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.Decisions
{
    internal sealed class EmployeeDecisionConfiguration : IEntityTypeConfiguration<EmployeeDecision>
    {
        public void Configure(EntityTypeBuilder<EmployeeDecision> builder)
        {
            // 1. Table Name
            builder.ToTable("EmployeeDecisions",Schemas.HR);

            // 2. Primary Key
            builder.HasKey(d => d.Id);

            // 3. Properties

            builder.Property(d => d.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(d => d.Notes)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(d => d.ValidFrom)
                .IsRequired(false);

            builder.Property(d => d.ValidTo)
                .IsRequired(false);

            builder.Property(d => d.Status)
                .HasConversion<string>() // Active / Ended / Cancelled
                .HasMaxLength(20)
                .IsRequired();

            builder.Ignore(d => d.IsActive);


            // 4. Foreign Keys

            builder.Property(d => d.EmployeeId)
                .IsRequired();

            builder.Property(d => d.DecisionId)
                .IsRequired();

            // 5. Relationships

            // Employee (1 → Many EmployeeDecisions)
            builder.HasOne<Employee>()
                .WithMany(e=>e.EmployeeDecisions)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decision (Lookup table)
            builder.HasOne<Decision>()
                .WithMany(e=>e.EmployeeDecisions)
                .HasForeignKey(d => d.DecisionId)
                .OnDelete(DeleteBehavior.Restrict);

            // 6. Indexes

            builder.HasIndex(d => d.EmployeeId);
            builder.HasIndex(d => d.DecisionId);
            builder.HasIndex(d => d.Status);
            builder.HasIndex(d => new { d.EmployeeId, d.Status });
        }
    }
}
