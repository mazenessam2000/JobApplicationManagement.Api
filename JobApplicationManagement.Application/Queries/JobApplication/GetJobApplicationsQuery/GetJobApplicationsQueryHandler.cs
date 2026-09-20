using JobApplicationManagement.Application.Contracts;
using JobApplicationManagement.Application.Exceptions;
using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Application.Services;
using MediatR;

namespace JobApplicationManagement.Application.Queries.JobApplication.GetJobApplicationsQuery;

public sealed class GetJobApplicationsQueryHandler(IJobRepository jobs, IJobApplicationQueries applications, ICurrentUserService currentUser) : IRequestHandler<GetJobApplicationsQuery, IReadOnlyList<JobApplicationDto>>
{
    public async Task<IReadOnlyList<JobApplicationDto>> Handle(GetJobApplicationsQuery request, CancellationToken cancellationToken)
    {
        var job = await jobs.GetByIdAsync(request.JobId, cancellationToken) ?? throw new KeyNotFoundException("Job was not found.");
        if (job.RecruiterId != currentUser.UserId) throw new ForbiddenException("You do not own this job.");
        return await applications.GetForJobAsync(job.Id, cancellationToken);
    }
}
