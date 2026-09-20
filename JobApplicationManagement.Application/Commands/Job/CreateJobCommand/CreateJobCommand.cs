using MediatR;

namespace JobApplicationManagement.Application.Commands.Job.CreateJobCommand;

public record CreateJobCommand(
    string Title,
    string Description,
    string Location,
    decimal? SalaryMin,
    decimal? SalaryMax
) : IRequest<Guid>;