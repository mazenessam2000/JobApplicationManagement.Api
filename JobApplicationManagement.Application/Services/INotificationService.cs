namespace JobApplicationManagement.Application.Services;

public interface INotificationService
{
    Task NotifyCandidateAsync(Guid applicationId, CancellationToken cancellationToken = default);
}
