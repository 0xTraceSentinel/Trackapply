using Trackapply.Application.JobApplications.Dtos;
using Trackapply.Domain.Entities;
using Trackapply.Domain.Enums;

namespace Trackapply.Application.JobApplications.Services;

public class JobApplicationService(IJobApplicationRepository repository) : IJobApplicationService
{
    public async Task<IReadOnlyList<JobApplicationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var jobApplications = await repository.GetAllAsync(cancellationToken);
        return jobApplications.Select(JobApplicationDto.FromEntity).ToList();
    }

    public async Task<JobApplicationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var jobApplication = await repository.GetByIdAsync(id, cancellationToken);
        return jobApplication is null ? null : JobApplicationDto.FromEntity(jobApplication);
    }

    public async Task<JobApplicationDto> CreateAsync(CreateJobApplicationDto dto, CancellationToken cancellationToken = default)
    {
        var jobApplication = new JobApplication(dto.Title, dto.Company, dto.CompanyUrl, dto.Description, dto.Status);

        repository.Add(jobApplication);
        await repository.SaveChangesAsync(cancellationToken);

        return JobApplicationDto.FromEntity(jobApplication);
    }

    public async Task<JobApplicationDto?> UpdateAsync(Guid id, UpdateJobApplicationDto dto, CancellationToken cancellationToken = default)
    {
        var jobApplication = await repository.GetByIdAsync(id, cancellationToken);
        if (jobApplication is null)
        {
            return null;
        }

        jobApplication.UpdateDetails(dto.Title, dto.Company, dto.CompanyUrl, dto.Description);
        await repository.SaveChangesAsync(cancellationToken);

        return JobApplicationDto.FromEntity(jobApplication);
    }

    public async Task<JobApplicationDto?> UpdateStatusAsync(Guid id, JobApplicationStatus status, CancellationToken cancellationToken = default)
    {
        var jobApplication = await repository.GetByIdAsync(id, cancellationToken);
        if (jobApplication is null)
        {
            return null;
        }

        jobApplication.ChangeStatus(status);
        await repository.SaveChangesAsync(cancellationToken);

        return JobApplicationDto.FromEntity(jobApplication);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var jobApplication = await repository.GetByIdAsync(id, cancellationToken);
        if (jobApplication is null)
        {
            return false;
        }

        repository.Remove(jobApplication);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
