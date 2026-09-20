using Microsoft.AspNetCore.Identity;

namespace JobApplicationManagement.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = null!;
}
