using JobApplicationManagement.Application.Contracts;
using MediatR;

namespace JobApplicationManagement.Application.Queries.Job.GetJobByIdQuery;

public sealed class GetJobByIdQueryHandler(IJobQueries jobs) : IRequestHandler<GetJobByIdQuery, JobDto>
{
    public async Task<JobDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken) =>
        await jobs.GetOpenByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException("Job was not found.");
}
