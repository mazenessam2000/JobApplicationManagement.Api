using JobApplicationManagement.Application.Contracts;
using JobApplicationManagement.Application.Services;
using MediatR;

namespace JobApplicationManagement.Application.Queries.JobApplication.GetMyApplicationsQuery;

public sealed class GetMyApplicationsQueryHandler(IJobApplicationQueries applications, ICurrentUserService currentUser) : IRequestHandler<GetMyApplicationsQuery, IReadOnlyList<JobApplicationDto>>
{
    public Task<IReadOnlyList<JobApplicationDto>> Handle(GetMyApplicationsQuery request, CancellationToken cancellationToken) =>
        applications.GetForCandidateAsync(currentUser.UserId, cancellationToken);
}
