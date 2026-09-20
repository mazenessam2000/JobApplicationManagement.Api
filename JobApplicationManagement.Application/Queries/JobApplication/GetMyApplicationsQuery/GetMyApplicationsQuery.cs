using JobApplicationManagement.Application.Contracts;
using MediatR;

namespace JobApplicationManagement.Application.Queries.JobApplication.GetMyApplicationsQuery;

public record GetMyApplicationsQuery : IRequest<IReadOnlyList<JobApplicationDto>>;
