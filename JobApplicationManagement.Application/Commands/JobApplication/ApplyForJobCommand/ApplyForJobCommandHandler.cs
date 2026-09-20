using JobApplicationManagement.Application.Exceptions;
using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Application.Services;
using JobApplicationManagement.Domain.Enums;
using MediatR;

namespace JobApplicationManagement.Application.Commands.JobApplication.ApplyForJobCommand;

public sealed class ApplyForJobCommandHandler(IJobRepository jobs, IJobApplicationRepository applications, IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<ApplyForJobCommand, Guid>
{
    public async Task<Guid> Handle(ApplyForJobCommand request, CancellationToken cancellationToken)
    {
        var job = await jobs.GetByIdAsync(request.JobId, cancellationToken) ?? throw new KeyNotFoundException("Job was not found.");
        if (!job.IsActive) throw new BusinessRuleException("Closed jobs cannot receive applications.");
        if (await applications.ExistsForCandidateAsync(job.Id, currentUser.UserId, cancellationToken))
            throw new BusinessRuleException("You have already applied to this job.");
        var application = new Domain.Entities.JobApplication(job.Id, currentUser.UserId, request.CoverLetter, request.ResumeUrl);
        applications.Add(application);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return application.Id;
    }
}
