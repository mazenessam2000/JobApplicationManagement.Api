using JobApplicationManagement.Application.Authorization;
using JobApplicationManagement.Application.Commands.JobApplication.CancelApplicationCommand;
using JobApplicationManagement.Application.Commands.JobApplication.UpdateApplicationStatusCommand;
using JobApplicationManagement.Application.Contracts;
using JobApplicationManagement.Application.Queries.JobApplication.GetMyApplicationsQuery;
using JobApplicationManagement.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationManagement.Api.Controllers;

[ApiController]
[Route("api/applications")]
public sealed class ApplicationsController(ISender sender) : ControllerBase
{
    [HttpGet("my")]
    [Authorize(Roles = Roles.Candidate)]
    public Task<IReadOnlyList<JobApplicationDto>> GetMine(CancellationToken cancellationToken) => sender.Send(new GetMyApplicationsQuery(), cancellationToken);

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Candidate)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new CancelApplicationCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = Roles.Recruiter)]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateApplicationStatusRequest request, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateApplicationStatusCommand(id, request.Status), cancellationToken);
        return NoContent();
    }
}

public sealed record UpdateApplicationStatusRequest(ApplicationStatus Status);
