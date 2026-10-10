using Trackapply.Domain.Enums;

namespace Trackapply.Domain.Entities;

public class JobApplication
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string Title { get; private set; } = string.Empty;
    public string Company { get; private set; } = string.Empty;
    public string? CompanyUrl { get; private set; }
    public string? Description { get; private set; }
    public JobApplicationStatus Status { get; private set; } = JobApplicationStatus.Bookmarked;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    private JobApplication() { }

    public JobApplication(
        string title,
        string company,
        string? companyUrl,
        string? description,
        JobApplicationStatus status = JobApplicationStatus.Bookmarked)
    {
        SetDetails(title, company, companyUrl, description);
        Status = status;
    }

    public void UpdateDetails(string title, string company, string? companyUrl, string? description)
    {
        SetDetails(title, company, companyUrl, description);
        Touch();
    }

    public void ChangeStatus(JobApplicationStatus status)
    {
        if (Status == status)
        {
            return;
        }

        Status = status;
        Touch();
    }

    private void SetDetails(string title, string company, string? companyUrl, string? description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(company);

        Title = title.Trim();
        Company = company.Trim();
        CompanyUrl = string.IsNullOrWhiteSpace(companyUrl) ? null : companyUrl.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
