using Microsoft.EntityFrameworkCore;
using Journal.Entities;

namespace Journal.Data;

public class JournalDbContext : DbContext
{
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    public JournalDbContext(DbContextOptions<JournalDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.Mood).HasMaxLength(50);
            entity.Property(e => e.Tags).HasMaxLength(500);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.Bio).HasMaxLength(500);
        });
    }
}
