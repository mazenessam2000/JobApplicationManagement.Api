using System.Security.Claims;
using JobApplicationManagement.Application.Services;
using Microsoft.AspNetCore.Http;

namespace JobApplicationManagement.Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId => Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
        ? userId
        : throw new UnauthorizedAccessException("An authenticated user is required.");
}
