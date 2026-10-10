using Trackapply.Application.JobApplications.Dtos;
using Trackapply.Domain.Enums;

namespace Trackapply.Application.JobApplications.Services;

public interface IJobApplicationService
{
    Task<IReadOnlyList<JobApplicationDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<JobApplicationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<JobApplicationDto> CreateAsync(CreateJobApplicationDto dto, CancellationToken cancellationToken = default);
    Task<JobApplicationDto?> UpdateAsync(Guid id, UpdateJobApplicationDto dto, CancellationToken cancellationToken = default);
    Task<JobApplicationDto?> UpdateStatusAsync(Guid id, JobApplicationStatus status, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
