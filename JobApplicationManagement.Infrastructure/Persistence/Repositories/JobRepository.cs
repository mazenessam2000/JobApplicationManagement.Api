using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationManagement.Infrastructure.Persistence.Repositories;

public sealed class JobRepository(ApplicationDbContext context) : IJobRepository
{
    public void Add(Job job) => context.Jobs.Add(job);

    public Task<Job?> GetByIdAsync(Guid jobId, CancellationToken cancellationToken) =>
        context.Jobs.SingleOrDefaultAsync(job => job.Id == jobId, cancellationToken);

    public async Task<IReadOnlyList<Job>> GetOpenCreatedBeforeAsync(DateTime cutoffUtc, CancellationToken cancellationToken) =>
        await context.Jobs
            .Where(job => job.IsActive && job.CreatedAt < cutoffUtc)
            .ToListAsync(cancellationToken);
}
