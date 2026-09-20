using MediatR;

namespace JobApplicationManagement.Application.Commands.JobApplication.CancelApplicationCommand;

public record CancelApplicationCommand(Guid Id) : IRequest;
