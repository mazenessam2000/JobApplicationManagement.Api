using MediatR;

namespace JobApplicationManagement.Application.Commands.JobApplication.ApplyForJobCommand;

public record ApplyForJobCommand(Guid JobId, string? CoverLetter, string? ResumeUrl) : IRequest<Guid>;
