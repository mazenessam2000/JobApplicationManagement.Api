using JobApplicationManagement.Application.Contracts;

namespace JobApplicationManagement.Application.Queries;

public interface IJobApplicationQueries
{
    Task<IReadOnlyList<JobApplicationDto>> GetForCandidateAsync(Guid candidateId, CancellationToken cancellationToken);
    Task<IReadOnlyList<JobApplicationDto>> GetForJobAsync(Guid jobId, CancellationToken cancellationToken);
}
