namespace Trackapply.Domain.Entities;

public class JobApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? CompanyUrl { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "Bookmarked"; // Bookmarked, Applied, Interviewing, Offered, Rejected
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}