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
    /// <summary>Gets applications submitted by the current candidate.</summary>
    [ProducesResponseType(typeof(IReadOnlyList<JobApplicationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public Task<IReadOnlyList<JobApplicationDto>> GetMine(CancellationToken cancellationToken) => sender.Send(new GetMyApplicationsQuery(), cancellationToken);

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Candidate)]
    /// <summary>Cancels an application owned by the current candidate.</summary>
    /// <remarks>Accepted and rejected applications cannot be cancelled.</remarks>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new CancelApplicationCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = Roles.Recruiter)]
    /// <summary>Updates the status of an application for a job owned by the current recruiter.</summary>
    /// <remarks>Final application statuses cannot be changed.</remarks>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateApplicationStatusRequest request, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateApplicationStatusCommand(id, request.Status), cancellationToken);
        return NoContent();
    }
}

public sealed record UpdateApplicationStatusRequest(ApplicationStatus Status);
