using JobApplicationManagement.Application.Exceptions;
using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Application.Services;
using JobApplicationManagement.Domain.Enums;
using MediatR;

namespace JobApplicationManagement.Application.Commands.JobApplication.UpdateApplicationStatusCommand;

public sealed class UpdateApplicationStatusCommandHandler(IJobApplicationRepository applications, IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<UpdateApplicationStatusCommand>
{
    public async Task Handle(UpdateApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var application = await applications.GetWithJobAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException("Application was not found.");
        if (application.Job.RecruiterId != currentUser.UserId) throw new ForbiddenException("You do not own this job.");
        if (application.Status is ApplicationStatus.Accepted or ApplicationStatus.Rejected)
            throw new BusinessRuleException("A finalised application status cannot be changed.");
        application.UpdateStatus(request.Status);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
