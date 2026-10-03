namespace Trackapply.Application.JobApplications.Dtos;

public class CreateJobApplicationDto
{
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? CompanyUrl { get; set; }
    public string? Description { get; set; }
}
