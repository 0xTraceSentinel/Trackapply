using System.ComponentModel.DataAnnotations;
using Trackapply.Domain.Enums;

namespace Trackapply.Application.JobApplications.Dtos;

public record UpdateJobApplicationStatusDto
{
    [Required]
    [EnumDataType(typeof(JobApplicationStatus))]
    public JobApplicationStatus? Status { get; init; }
}
