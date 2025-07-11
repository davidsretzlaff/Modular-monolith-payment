using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.EntityConfigurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("Plans");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CompanyId)
            .IsRequired();

        builder.Property(p => p.DurationInDays)
            .IsRequired()
            .HasDefaultValue(30);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(p => p.CompanyId)
            .HasDatabaseName("IX_Plans_CompanyId");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Plans_IsActive");

        builder.HasIndex(p => new { p.CompanyId, p.IsActive })
            .HasDatabaseName("IX_Plans_CompanyId_IsActive");

        builder.HasIndex(p => p.Name)
            .HasDatabaseName("IX_Plans_Name");

        // Configuração da coleção de PlanPricingOption
        builder.HasMany(x => x.PricingOptions)
            .WithOne(x => x.Plan)
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 