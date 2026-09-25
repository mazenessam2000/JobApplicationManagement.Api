namespace JobApplicationManagement.Application.Services;

public interface IBackgroundJobScheduler
{
    void EnqueueCandidateCancellationNotification(Guid applicationId);
}
