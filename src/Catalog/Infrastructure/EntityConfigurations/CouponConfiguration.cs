using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.EntityConfigurations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.DiscountValue)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.IsPercentage)
            .IsRequired();

        builder.Property(c => c.ValidFrom)
            .IsRequired();

        builder.Property(c => c.ValidUntil)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(c => c.UsageLimit)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(c => c.UsedCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.Property(c => c.PlanId)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("IX_Coupons_Code");

        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_Coupons_IsActive");

        builder.HasIndex(c => new { c.ValidFrom, c.ValidUntil })
            .HasDatabaseName("IX_Coupons_ValidPeriod");

        builder.HasIndex(c => c.PlanId)
            .HasDatabaseName("IX_Coupons_PlanId");
    }
} 