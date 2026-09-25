using JobApplicationManagement.Application.Exceptions;
using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Application.Services;
using JobApplicationManagement.Domain.Enums;
using MediatR;

namespace JobApplicationManagement.Application.Commands.JobApplication.CancelApplicationCommand;

public sealed class CancelApplicationCommandHandler(
    IJobApplicationRepository applications,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser,
    IBackgroundJobScheduler backgroundJobs) : IRequestHandler<CancelApplicationCommand>
{
    public async Task Handle(CancelApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await applications.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException("Application was not found.");
        if (application.CandidateId != currentUser.UserId) throw new ForbiddenException("You do not own this application.");
        if (application.Status is ApplicationStatus.Accepted or ApplicationStatus.Rejected)
            throw new BusinessRuleException("A finalised application cannot be cancelled.");
        applications.Remove(application);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        backgroundJobs.EnqueueCandidateCancellationNotification(application.Id);
    }
}
