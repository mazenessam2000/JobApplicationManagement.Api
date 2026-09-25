using System.ComponentModel.DataAnnotations;

namespace JobApplicationManagement.Infrastructure.Services;

public sealed class JobLifecycleOptions
{
    public const string SectionName = "JobLifecycle";
    [Range(1, 3650)]
    public int AutoCloseAfterDays { get; init; } = 30;
}
