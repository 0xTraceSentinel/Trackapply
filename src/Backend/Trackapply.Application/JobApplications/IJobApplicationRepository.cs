using Trackapply.Domain.Entities;

namespace Trackapply.Application.JobApplications;

public interface IJobApplicationRepository
{
    Task<IReadOnlyList<JobApplication>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(JobApplication jobApplication);
    void Remove(JobApplication jobApplication);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
