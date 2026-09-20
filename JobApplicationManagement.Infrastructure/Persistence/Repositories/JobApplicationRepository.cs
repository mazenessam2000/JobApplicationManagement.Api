using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationManagement.Infrastructure.Persistence.Repositories;

public sealed class JobApplicationRepository(ApplicationDbContext context) : IJobApplicationRepository
{
    public void Add(JobApplication application) => context.JobApplications.Add(application);

    public Task<JobApplication?> GetByIdAsync(Guid applicationId, CancellationToken cancellationToken) =>
        context.JobApplications.SingleOrDefaultAsync(application => application.Id == applicationId, cancellationToken);

    public Task<JobApplication?> GetWithJobAsync(Guid applicationId, CancellationToken cancellationToken) =>
        context.JobApplications.Include(application => application.Job)
            .SingleOrDefaultAsync(application => application.Id == applicationId, cancellationToken);

    public Task<bool> ExistsForCandidateAsync(Guid jobId, Guid candidateId, CancellationToken cancellationToken) =>
        context.JobApplications.AnyAsync(application => application.JobId == jobId && application.CandidateId == candidateId, cancellationToken);

    public void Remove(JobApplication application) => context.JobApplications.Remove(application);
}
