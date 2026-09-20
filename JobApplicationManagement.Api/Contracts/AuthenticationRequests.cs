using System.ComponentModel.DataAnnotations;
using JobApplicationManagement.Application.Authorization;

namespace JobApplicationManagement.Api.Contracts;

public sealed class RegisterRequest
{
    [Required, StringLength(200)]
    public string FullName { get; init; } = string.Empty;

    [Required, EmailAddress, StringLength(256)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(128, MinimumLength = 12)]
    public string Password { get; init; } = string.Empty;

    [Required]
    [RegularExpression($"{Roles.Candidate}|{Roles.Recruiter}", ErrorMessage = "Role must be Candidate or Recruiter.")]
    public string Role { get; init; } = string.Empty;
}

public sealed class LoginRequest
{
    [Required, EmailAddress, StringLength(256)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(128, MinimumLength = 1)]
    public string Password { get; init; } = string.Empty;
}
