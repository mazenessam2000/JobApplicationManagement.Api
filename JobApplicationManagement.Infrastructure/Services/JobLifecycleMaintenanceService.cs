using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JobApplicationManagement.Infrastructure.Services;

public sealed class JobLifecycleMaintenanceService(
    IJobRepository jobs,
    IUnitOfWork unitOfWork,
    IOptions<JobLifecycleOptions> options,
    ILogger<JobLifecycleMaintenanceService> logger) : IJobLifecycleMaintenanceService
{
    public async Task CloseExpiredJobsAsync(CancellationToken cancellationToken = default)
    {
        var cutoffUtc = DateTime.UtcNow.AddDays(-options.Value.AutoCloseAfterDays);
        var expiredJobs = await jobs.GetOpenCreatedBeforeAsync(cutoffUtc, cancellationToken);

        foreach (var job in expiredJobs)
            job.Close();

        if (expiredJobs.Count == 0)
            return;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Closed {JobCount} job(s) created before {CutoffUtc}.", expiredJobs.Count, cutoffUtc);
    }
}
