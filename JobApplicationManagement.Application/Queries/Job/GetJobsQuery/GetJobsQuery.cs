using JobApplicationManagement.Application.Contracts;
using MediatR;

namespace JobApplicationManagement.Application.Queries.Job.GetJobsQuery;

public record GetJobsQuery : IRequest<IReadOnlyList<JobDto>>;
