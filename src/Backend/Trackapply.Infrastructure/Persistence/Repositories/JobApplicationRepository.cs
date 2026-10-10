using Microsoft.EntityFrameworkCore;
using Trackapply.Application.JobApplications;
using Trackapply.Domain.Entities;

namespace Trackapply.Infrastructure.Persistence.Repositories;

public class JobApplicationRepository(ApplicationDbContext dbContext) : IJobApplicationRepository
{
    public async Task<IReadOnlyList<JobApplication>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.JobApplications
            .AsNoTracking()
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);

    public Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.JobApplications.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public void Add(JobApplication jobApplication) => dbContext.JobApplications.Add(jobApplication);

    public void Remove(JobApplication jobApplication) => dbContext.JobApplications.Remove(jobApplication);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
