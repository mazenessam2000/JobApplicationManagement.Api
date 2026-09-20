using JobApplicationManagement.Domain.Enums;

namespace JobApplicationManagement.Application.Contracts;

public sealed record JobApplicationDto(Guid Id, Guid JobId, string JobTitle, ApplicationStatus Status, DateTime AppliedAt, string? CoverLetter, string? ResumeUrl);
