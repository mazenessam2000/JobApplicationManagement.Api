using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Domain.Entities;
using JobApplicationManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ApplicationRoles = JobApplicationManagement.Application.Authorization.Roles;

namespace JobApplicationManagement.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IUnitOfWork
{
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        builder.Entity<ApplicationUser>().Property(user => user.FullName).HasMaxLength(200).IsRequired();
        builder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid> { Id = Guid.Parse("5c4c0f01-d8f8-4ab7-a012-86d4de2aa001"), Name = ApplicationRoles.Candidate, NormalizedName = ApplicationRoles.Candidate.ToUpperInvariant(), ConcurrencyStamp = "76e0d04a-7eb2-46e9-b1b8-25fe4a02d4f1" },
            new IdentityRole<Guid> { Id = Guid.Parse("5c4c0f01-d8f8-4ab7-a012-86d4de2aa002"), Name = ApplicationRoles.Recruiter, NormalizedName = ApplicationRoles.Recruiter.ToUpperInvariant(), ConcurrencyStamp = "526ae72f-4924-434a-9616-af43de3a517b" });
    }
}
