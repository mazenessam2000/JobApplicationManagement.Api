using JobApplicationManagement.Application.Contracts;
using MediatR;

namespace JobApplicationManagement.Application.Queries.Job.GetJobsQuery;

public sealed class GetJobsQueryHandler(IJobQueries jobs) : IRequestHandler<GetJobsQuery, IReadOnlyList<JobDto>>
{
    public Task<IReadOnlyList<JobDto>> Handle(GetJobsQuery request, CancellationToken cancellationToken) => jobs.GetOpenJobsAsync(cancellationToken);
}
