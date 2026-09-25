using JobApplicationManagement.Application.Services;
using Microsoft.Extensions.Logging;

namespace JobApplicationManagement.Infrastructure.Services;

public sealed class LoggingNotificationService(ILogger<LoggingNotificationService> logger) : INotificationService
{
    public Task NotifyCandidateAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Application {ApplicationId} was cancelled; candidate notification was requested.", applicationId);
        return Task.CompletedTask;
    }
}
