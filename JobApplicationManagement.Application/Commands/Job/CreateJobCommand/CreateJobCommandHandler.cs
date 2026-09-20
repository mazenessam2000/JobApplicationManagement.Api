using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Application.Services;
using MediatR;

namespace JobApplicationManagement.Application.Commands.Job.CreateJobCommand;

public sealed class CreateJobCommandHandler(IJobRepository jobs, IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<CreateJobCommand, Guid>
{
    public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = new Domain.Entities.Job(request.Title, request.Description, request.Location,
            request.SalaryMin, request.SalaryMax, currentUser.UserId);
        jobs.Add(job);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return job.Id;
    }
}
