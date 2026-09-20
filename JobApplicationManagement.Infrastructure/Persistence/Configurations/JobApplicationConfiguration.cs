using JobApplicationManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobApplicationManagement.Infrastructure.Persistence.Configurations;

public sealed class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications");
        builder.HasKey(application => application.Id);
        builder.Property(application => application.Id).ValueGeneratedNever();
        builder.Property(application => application.CoverLetter).HasMaxLength(5000);
        builder.Property(application => application.ResumeUrl).HasMaxLength(2048);
        builder.HasIndex(application => new { application.JobId, application.CandidateId }).IsUnique();
        builder.HasOne(application => application.Job).WithMany(job => job.Applications)
            .HasForeignKey(application => application.JobId).OnDelete(DeleteBehavior.Cascade);
    }
}
