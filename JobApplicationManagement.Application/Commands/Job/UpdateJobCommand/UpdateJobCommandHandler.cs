using JobApplicationManagement.Application.Exceptions;
using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Application.Services;
using MediatR;

namespace JobApplicationManagement.Application.Commands.Job.UpdateJobCommand;

public sealed class UpdateJobCommandHandler(IJobRepository jobs, IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<UpdateJobCommand>
{
    public async Task Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        var job = await jobs.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException("Job was not found.");
        if (job.RecruiterId != currentUser.UserId) throw new ForbiddenException("You do not own this job.");
        if (!job.IsActive) throw new BusinessRuleException("A closed job cannot be updated.");
        job.Update(request.Title, request.Description, request.Location, request.SalaryMin, request.SalaryMax);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
