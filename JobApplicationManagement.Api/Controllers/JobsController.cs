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
    public Task<IReadOnlyList<JobDto>> Get(CancellationToken cancellationToken) => sender.Send(new GetJobsQuery(), cancellationToken);

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public Task<JobDto> GetById(Guid id, CancellationToken cancellationToken) => sender.Send(new GetJobByIdQuery(id), cancellationToken);

    [HttpPost]
    [Authorize(Roles = Roles.Recruiter)]
    public async Task<IActionResult> Create(CreateJobCommand command, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Recruiter)]
    public async Task<IActionResult> Update(Guid id, UpdateJobRequest request, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateJobCommand(id, request.Title, request.Description, request.Location, request.SalaryMin, request.SalaryMax), cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/close")]
    [Authorize(Roles = Roles.Recruiter)]
    public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new CloseJobCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{jobId:guid}/applications")]
    [Authorize(Roles = Roles.Candidate)]
    public async Task<IActionResult> Apply(Guid jobId, ApplyForJobRequest request, CancellationToken cancellationToken)
    {
        var id = await sender.Send(new ApplyForJobCommand(jobId, request.CoverLetter, request.ResumeUrl), cancellationToken);
        return Created($"/api/applications/{id}", new { id });
    }

    [HttpGet("{jobId:guid}/applications")]
    [Authorize(Roles = Roles.Recruiter)]
    public Task<IReadOnlyList<JobApplicationDto>> GetApplications(Guid jobId, CancellationToken cancellationToken) =>
        sender.Send(new GetJobApplicationsQuery(jobId), cancellationToken);
}

public sealed record UpdateJobRequest(string Title, string Description, string Location, decimal? SalaryMin, decimal? SalaryMax);
public sealed record ApplyForJobRequest(string? CoverLetter, string? ResumeUrl);
