using JobApplicationManagement.Domain.Entities;

namespace JobApplicationManagement.Application.Persistence;

public interface IJobRepository
{
    void Add(Job job);
    Task<Job?> GetByIdAsync(Guid jobId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Job>> GetOpenCreatedBeforeAsync(DateTime cutoffUtc, CancellationToken cancellationToken);
}
