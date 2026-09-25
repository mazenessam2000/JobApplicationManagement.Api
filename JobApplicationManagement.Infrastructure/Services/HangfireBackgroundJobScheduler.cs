using Hangfire;
using JobApplicationManagement.Application.Services;

namespace JobApplicationManagement.Infrastructure.Services;

public sealed class HangfireBackgroundJobScheduler(IBackgroundJobClient backgroundJobs) : IBackgroundJobScheduler
{
    public void EnqueueCandidateCancellationNotification(Guid applicationId) =>
        backgroundJobs.Enqueue<INotificationService>(service =>
            service.NotifyCandidateAsync(applicationId, CancellationToken.None));
}
