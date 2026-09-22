using JobApplicationManagement.Application.Authorization;
using JobApplicationManagement.Application.Commands.Job.CloseJobCommand;
using JobApplicationManagement.Application.Commands.Job.CreateJobCommand;
using JobApplicationManagement.Application.Commands.Job.UpdateJobCommand;
using JobApplicationManagement.Application.Commands.JobApplication.ApplyForJobCommand;
using JobApplicationManagement.Application.Contracts;
using JobApplicationManagement.Application.Queries.Job.GetJobByIdQuery;
using JobApplicationManagement.Application.Queries.Job.GetJobsQuery;
using JobApplicationManagement.Application.Queries.JobApplication.GetJobApplicationsQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationManagement.Api.Controllers;

[ApiController]
[Route("api/jobs")]
public sealed class JobsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    /// <summary>Gets all open jobs available for applications.</summary>
    [ProducesResponseType(typeof(IReadOnlyList<JobDto>), StatusCodes.Status200OK)]
    public Task<IReadOnlyList<JobDto>> Get(CancellationToken cancellationToken) => sender.Send(new GetJobsQuery(), cancellationToken);

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    /// <summary>Gets an open job by its identifier.</summary>
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<JobDto> GetById(Guid id, CancellationToken cancellationToken) => sender.Send(new GetJobByIdQuery(id), cancellationToken);

    [HttpPost]
    [Authorize(Roles = Roles.Recruiter)]
    /// <summary>Creates a new job for the current recruiter.</summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(CreateJobCommand command, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Recruiter)]
    /// <summary>Updates a job owned by the current recruiter.</summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, UpdateJobRequest request, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateJobCommand(id, request.Title, request.Description, request.Location, request.SalaryMin, request.SalaryMax), cancellationToken);
        return NoContent();
    }

    /// <summary>Closes a job owned by the current recruiter.</summary>
    /// <remarks>A closed job cannot accept new applications and cannot be closed again.</remarks>
    /// <param name="id">The identifier of the job to close.</param>
    /// <param name="cancellationToken">The request cancellation token.</param>
    /// <response code="204">The job was closed successfully.</response>
    /// <response code="401">The caller is not authenticated.</response>
    /// <response code="403">The current recruiter does not own the job.</response>
    /// <response code="404">The job does not exist.</response>
    /// <response code="409">The job is already closed.</response>
    [HttpPut("{id:guid}/close")]
    [Authorize(Roles = Roles.Recruiter)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new CloseJobCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{jobId:guid}/applications")]
    [Authorize(Roles = Roles.Candidate)]
    /// <summary>Submits an application for an open job as the current candidate.</summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Apply(Guid jobId, ApplyForJobRequest request, CancellationToken cancellationToken)
    {
        var id = await sender.Send(new ApplyForJobCommand(jobId, request.CoverLetter, request.ResumeUrl), cancellationToken);
        return Created($"/api/applications/{id}", new { id });
    }

    [HttpGet("{jobId:guid}/applications")]
    [Authorize(Roles = Roles.Recruiter)]
    /// <summary>Gets applications for a job owned by the current recruiter.</summary>
    [ProducesResponseType(typeof(IReadOnlyList<JobApplicationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IReadOnlyList<JobApplicationDto>> GetApplications(Guid jobId, CancellationToken cancellationToken) =>
        sender.Send(new GetJobApplicationsQuery(jobId), cancellationToken);
}

public sealed record UpdateJobRequest(string Title, string Description, string Location, decimal? SalaryMin, decimal? SalaryMax);
public sealed record ApplyForJobRequest(string? CoverLetter, string? ResumeUrl);
