using JobApplicationManagement.Application.Contracts;
using JobApplicationManagement.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationManagement.Infrastructure.Persistence.Queries;

public sealed class JobQueries(ApplicationDbContext context) : IJobQueries
{
    public async Task<IReadOnlyList<JobDto>> GetOpenJobsAsync(CancellationToken cancellationToken) => await context.Jobs.AsNoTracking()
        .Where(job => job.IsActive).OrderByDescending(job => job.CreatedAt)
        .Select(job => new JobDto(job.Id, job.Title, job.Description, job.Location, job.SalaryMin, job.SalaryMax, job.IsActive, job.CreatedAt, job.UpdatedAt))
        .ToListAsync(cancellationToken);

    public Task<JobDto?> GetOpenByIdAsync(Guid jobId, CancellationToken cancellationToken) => context.Jobs.AsNoTracking()
        .Where(job => job.Id == jobId && job.IsActive)
        .Select(job => new JobDto(job.Id, job.Title, job.Description, job.Location, job.SalaryMin, job.SalaryMax, job.IsActive, job.CreatedAt, job.UpdatedAt))
        .SingleOrDefaultAsync(cancellationToken);
}
