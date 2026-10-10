using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackapply.Domain.Constants;
using Trackapply.Domain.Entities;
using Trackapply.Domain.Enums;

namespace Trackapply.Infrastructure.Persistence.Configurations;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(JobApplicationConstraints.TitleMaxLength);

        builder.Property(e => e.Company)
            .IsRequired()
            .HasMaxLength(JobApplicationConstraints.CompanyMaxLength);

        builder.Property(e => e.CompanyUrl)
            .HasMaxLength(JobApplicationConstraints.CompanyUrlMaxLength);

        builder.Property(e => e.Description)
            .HasMaxLength(JobApplicationConstraints.DescriptionMaxLength);

        // Stored as text so the column stays readable and enum reordering can't corrupt data.
        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(e => e.Status);

        builder.ToTable(table => table.HasCheckConstraint(
            "ck_job_applications_status",
            $"status IN ({string.Join(", ", Enum.GetNames<JobApplicationStatus>().Select(s => $"'{s}'"))})"));
    }
}
