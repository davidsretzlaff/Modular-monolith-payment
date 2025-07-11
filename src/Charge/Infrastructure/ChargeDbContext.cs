using Microsoft.EntityFrameworkCore;
using Charge.Domain.Entities;

namespace Charge.Infrastructure;

public class ChargeDbContext : DbContext
{
    public ChargeDbContext(DbContextOptions<ChargeDbContext> options) : base(options)
    {
    }

    public DbSet<Sale> Sales { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurações das entidades serão aplicadas automaticamente
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChargeDbContext).Assembly);
    }
} 