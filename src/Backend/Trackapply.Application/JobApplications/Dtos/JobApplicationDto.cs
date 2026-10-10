using Trackapply.Domain.Entities;
using Trackapply.Domain.Enums;

namespace Trackapply.Application.JobApplications.Dtos;

public record JobApplicationDto(
    Guid Id,
    string Title,
    string Company,
    string? CompanyUrl,
    string? Description,
    JobApplicationStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static JobApplicationDto FromEntity(JobApplication entity) => new(
        entity.Id,
        entity.Title,
        entity.Company,
        entity.CompanyUrl,
        entity.Description,
        entity.Status,
        entity.CreatedAt,
        entity.UpdatedAt);
}
