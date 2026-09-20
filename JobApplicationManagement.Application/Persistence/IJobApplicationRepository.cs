using JobApplicationManagement.Domain.Entities;

namespace JobApplicationManagement.Application.Persistence;

public interface IJobApplicationRepository
{
    void Add(JobApplication application);
    Task<JobApplication?> GetByIdAsync(Guid applicationId, CancellationToken cancellationToken);
    Task<JobApplication?> GetWithJobAsync(Guid applicationId, CancellationToken cancellationToken);
    Task<bool> ExistsForCandidateAsync(Guid jobId, Guid candidateId, CancellationToken cancellationToken);
    void Remove(JobApplication application);
}
