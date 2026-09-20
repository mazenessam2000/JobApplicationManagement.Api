using JobApplicationManagement.Application.Contracts;
using MediatR;

namespace JobApplicationManagement.Application.Queries.Job.GetJobByIdQuery;

public record GetJobByIdQuery(Guid Id) : IRequest<JobDto>;
