using JobApplicationManagement.Api.Authentication;
using JobApplicationManagement.Api.Contracts;
using JobApplicationManagement.Application.Authorization;
using JobApplicationManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationManagement.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public sealed class AuthController(UserManager<ApplicationUser> userManager, JwtTokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    /// <summary>Registers a Candidate or Recruiter and returns a JWT access token.</summary>
    /// <remarks>The email must be unique and the requested role must be supported by the application.</remarks>
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var email = request.Email.Trim();
        if (await userManager.FindByEmailAsync(email) is not null)
            return Conflict(new ProblemDetails { Title = "Registration failed", Detail = "A user with this email already exists." });

        var user = new ApplicationUser { Id = Guid.NewGuid(), FullName = request.FullName.Trim(), Email = email, UserName = email };
        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
            return ValidationProblem(new ValidationProblemDetails(createResult.Errors.ToDictionary(error => error.Code, error => new[] { error.Description })));

        var roleResult = await userManager.AddToRoleAsync(user, request.Role);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Registration failed");
        }

        return StatusCode(StatusCodes.Status201Created, await tokenService.CreateAsync(user, userManager));
    }

    [HttpPost("login")]
    /// <summary>Authenticates a user and returns a JWT access token.</summary>
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AccessTokenResponse>> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized();

        return Ok(await tokenService.CreateAsync(user, userManager));
    }
}
