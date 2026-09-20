using MediatR;

namespace JobApplicationManagement.Application.Commands.Job.CloseJobCommand;

public record CloseJobCommand(Guid Id) : IRequest;
