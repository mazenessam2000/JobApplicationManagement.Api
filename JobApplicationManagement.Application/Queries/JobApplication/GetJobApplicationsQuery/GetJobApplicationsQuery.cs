using JobApplicationManagement.Application.Contracts;
using MediatR;

namespace JobApplicationManagement.Application.Queries.JobApplication.GetJobApplicationsQuery;

public record GetJobApplicationsQuery(Guid JobId) : IRequest<IReadOnlyList<JobApplicationDto>>;
