namespace JobApplicationManagement.Application.Contracts;

public sealed record JobDto(Guid Id, string Title, string Description, string Location, decimal? SalaryMin, decimal? SalaryMax, bool IsActive, DateTime CreatedAt, DateTime? UpdatedAt);
