using Microsoft.EntityFrameworkCore;
using Data.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Users>? Users { get; set; }
    public DbSet<ApplicationUser>? ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Users>()
            .HasIndex(u => u.Nombre)
            .IsUnique();

        modelBuilder.Entity<Users>()
            .HasIndex(u => u.Uuid)
            .IsUnique();

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(u => u.Uuid)
            .IsUnique();
    }
}
