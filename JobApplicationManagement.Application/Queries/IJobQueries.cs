using JobApplicationManagement.Application.Contracts;

namespace JobApplicationManagement.Application.Queries;

public interface IJobQueries
{
    Task<IReadOnlyList<JobDto>> GetOpenJobsAsync(CancellationToken cancellationToken);
    Task<JobDto?> GetOpenByIdAsync(Guid jobId, CancellationToken cancellationToken);
}
