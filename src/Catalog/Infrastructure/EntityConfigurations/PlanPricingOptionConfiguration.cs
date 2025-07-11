using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.EntityConfigurations;

public class PlanPricingOptionConfiguration : IEntityTypeConfiguration<PlanPricingOption>
{
    public void Configure(EntityTypeBuilder<PlanPricingOption> builder)
    {
        builder.ToTable("PlanPricingOptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.BillingCycleInMonths)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreateDate)
            .IsRequired();

        // Relacionamento com Plan
        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 