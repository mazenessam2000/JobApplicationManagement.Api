using JobApplicationManagement.Application.Contracts;
using JobApplicationManagement.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationManagement.Infrastructure.Persistence.Queries;

public sealed class JobApplicationQueries(ApplicationDbContext context) : IJobApplicationQueries
{
    public async Task<IReadOnlyList<JobApplicationDto>> GetForCandidateAsync(Guid candidateId, CancellationToken cancellationToken) => 
        await context.JobApplications.AsNoTracking()
        .Where(application => application.CandidateId == candidateId)
        .OrderByDescending(application => application.AppliedAt)
        .Select(application => new JobApplicationDto(application.Id, application.JobId, application.Job.Title, application.Status, application.AppliedAt, application.CoverLetter, application.ResumeUrl))
        .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<JobApplicationDto>> GetForJobAsync(Guid jobId, CancellationToken cancellationToken) => 
        await context.JobApplications.AsNoTracking()
        .Where(application => application.JobId == jobId)
        .OrderByDescending(application => application.AppliedAt)
        .Select(application => new JobApplicationDto(application.Id, application.JobId, application.Job.Title, application.Status, application.AppliedAt, application.CoverLetter, application.ResumeUrl))
        .ToListAsync(cancellationToken);
}
