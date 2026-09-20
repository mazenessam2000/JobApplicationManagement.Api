using MediatR;

namespace JobApplicationManagement.Application.Commands.Job.UpdateJobCommand;

public record UpdateJobCommand(Guid Id, string Title, string Description, string Location, decimal? SalaryMin, decimal? SalaryMax) : IRequest;
