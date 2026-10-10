using Microsoft.AspNetCore.Mvc;
using Trackapply.Application.JobApplications.Dtos;
using Trackapply.Application.JobApplications.Services;

namespace Trackapply.Api.Controllers;

[ApiController]
[Route("api/job-applications")]
[Produces("application/json")]
public class JobApplicationsController(IJobApplicationService jobApplicationService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<JobApplicationDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<JobApplicationDto>>> GetAll(CancellationToken cancellationToken)
    {
        var jobApplications = await jobApplicationService.GetAllAsync(cancellationToken);
        return Ok(jobApplications);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<JobApplicationDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobApplicationDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var jobApplication = await jobApplicationService.GetByIdAsync(id, cancellationToken);
        return jobApplication is null ? NotFound() : Ok(jobApplication);
    }

    [HttpPost]
    [ProducesResponseType<JobApplicationDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JobApplicationDto>> Create(
        CreateJobApplicationDto dto,
        CancellationToken cancellationToken)
    {
        var jobApplication = await jobApplicationService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = jobApplication.Id }, jobApplication);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<JobApplicationDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobApplicationDto>> Update(
        Guid id,
        UpdateJobApplicationDto dto,
        CancellationToken cancellationToken)
    {
        var jobApplication = await jobApplicationService.UpdateAsync(id, dto, cancellationToken);
        return jobApplication is null ? NotFound() : Ok(jobApplication);
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType<JobApplicationDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobApplicationDto>> UpdateStatus(
        Guid id,
        UpdateJobApplicationStatusDto dto,
        CancellationToken cancellationToken)
    {
        var jobApplication = await jobApplicationService.UpdateStatusAsync(id, dto.Status!.Value, cancellationToken);
        return jobApplication is null ? NotFound() : Ok(jobApplication);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await jobApplicationService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
