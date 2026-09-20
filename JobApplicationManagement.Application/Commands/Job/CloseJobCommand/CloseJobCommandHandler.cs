using JobApplicationManagement.Application.Exceptions;
using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Application.Services;
using MediatR;

namespace JobApplicationManagement.Application.Commands.Job.CloseJobCommand;

public sealed class CloseJobCommandHandler(IJobRepository jobs, IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<CloseJobCommand>
{
    public async Task Handle(CloseJobCommand request, CancellationToken cancellationToken)
    {
        var job = await jobs.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException("Job was not found.");
        if (job.RecruiterId != currentUser.UserId) throw new ForbiddenException("You do not own this job.");
        if (!job.IsActive) throw new BusinessRuleException("Job is already closed.");
        job.Close();
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
