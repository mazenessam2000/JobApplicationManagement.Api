using JobApplicationManagement.Domain.Enums;
using MediatR;

namespace JobApplicationManagement.Application.Commands.JobApplication.UpdateApplicationStatusCommand;

public record UpdateApplicationStatusCommand(Guid Id, ApplicationStatus Status) : IRequest;
