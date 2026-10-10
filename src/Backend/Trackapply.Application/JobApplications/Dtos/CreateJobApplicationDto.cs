using System.ComponentModel.DataAnnotations;
using Trackapply.Domain.Constants;
using Trackapply.Domain.Enums;

namespace Trackapply.Application.JobApplications.Dtos;

public record CreateJobApplicationDto
{
    [Required]
    [MaxLength(JobApplicationConstraints.TitleMaxLength)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [MaxLength(JobApplicationConstraints.CompanyMaxLength)]
    public string Company { get; init; } = string.Empty;

    [Url]
    [MaxLength(JobApplicationConstraints.CompanyUrlMaxLength)]
    public string? CompanyUrl { get; init; }

    [MaxLength(JobApplicationConstraints.DescriptionMaxLength)]
    public string? Description { get; init; }

    [EnumDataType(typeof(JobApplicationStatus))]
    public JobApplicationStatus Status { get; init; } = JobApplicationStatus.Bookmarked;
}
