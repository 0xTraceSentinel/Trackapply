using System.ComponentModel.DataAnnotations;
using Trackapply.Domain.Constants;

namespace Trackapply.Application.JobApplications.Dtos;

public record UpdateJobApplicationDto
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
}
