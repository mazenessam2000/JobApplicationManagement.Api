using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobApplicationManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JobApplicationManagement.Api.Authentication;

public sealed class JwtTokenService(IOptions<JwtOptions> jwtOptions)
{
    public async Task<AccessTokenResponse> CreateAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
    {
        var options = jwtOptions.Value;
        var expiresAt = DateTime.UtcNow.AddMinutes(options.ExpirationMinutes);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        foreach (var role in await userManager.GetRolesAsync(user))
            claims.Add(new Claim(ClaimTypes.Role, role));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

        return new AccessTokenResponse(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}

public sealed record AccessTokenResponse(string AccessToken, DateTime ExpiresAtUtc);
