using JobApplicationManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobApplicationManagement.Infrastructure.Persistence.Configurations;

public sealed class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs");
        builder.HasKey(job => job.Id);
        builder.Property(job => job.Id).ValueGeneratedNever();
        builder.Property(job => job.Title).HasMaxLength(200).IsRequired();
        builder.Property(job => job.Description).HasMaxLength(5000).IsRequired();
        builder.Property(job => job.Location).HasMaxLength(200).IsRequired();
        builder.Property(job => job.SalaryMin).HasPrecision(18, 2);
        builder.Property(job => job.SalaryMax).HasPrecision(18, 2);
        builder.HasIndex(job => new { job.RecruiterId, job.IsActive });
    }
}
