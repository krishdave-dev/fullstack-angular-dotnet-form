using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .HasDefaultValueSql("NEWSEQUENTIALID()");

            entity.Property(x => x.FirstName)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(x => x.LastName)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(x => x.Email)
                  .HasMaxLength(255)
                  .IsRequired();

            entity.Property(x => x.City)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(x => x.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(x => x.IsDeleted)
                  .HasDefaultValue(false);

            entity.HasIndex(x => x.Email)
                  .IsUnique();

            entity.HasIndex(x =>
                new
                {
                    x.FirstName,
                    x.LastName,
                    x.City,
                    x.IsDeleted
                });

            // Global Soft Delete Filter
            entity.HasQueryFilter(x => !x.IsDeleted);
        });
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var entries =
            ChangeTracker
            .Entries<Customer>();

        foreach(var entry in entries)
        {
            if(entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt =
                    DateTime.UtcNow;
            }

            if(entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt =
                    DateTime.UtcNow;
            }
        }

        return await base
            .SaveChangesAsync(cancellationToken);
    }
}