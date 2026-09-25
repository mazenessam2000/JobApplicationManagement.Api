namespace JobApplicationManagement.Application.Services;

public interface IJobLifecycleMaintenanceService
{
    Task CloseExpiredJobsAsync(CancellationToken cancellationToken = default);
}
