using Microsoft.EntityFrameworkCore;

namespace TheProjector.Application.Persistence;

public class DatabaseContext : DbContext
{
  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

  public DbSet<Role> Roles { get; set; }

  public DbSet<User> Users { get; set; }

  public DbSet<Person> Persons { get; set; }

  public DbSet<Project> Projects { get; set; }

  public DbSet<ProjectAssignment> ProjectAssignments { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.Entity<Role>()
    .HasData(
      new Role { Id = new Guid("e27b8fa8-6c20-414f-bd0a-d9c98f808d25"), Name = "Administrator" },
      new Role { Id = new Guid("d48c2db8-da40-4336-9475-e8dd42b306d7"), Name = "Manager" },
      new Role { Id = new Guid("7f13ba44-9a9d-4eac-9615-98dc610c5563"), Name = "Employee" });

    builder.Entity<Status>()
    .HasData(
      new Status { Id = new Guid("ca7a477d-ab7a-41e1-8d0c-86a91efd219d"), Name = "Active" },
      new Status { Id = new Guid("95130666-173e-41d2-bfa1-6faae47355f8"), Name = "Pending" },
      new Status { Id = new Guid("4ece0109-f9b9-4f55-822b-1524508e27f6"), Name = "Disabled" });

    builder.Entity<Project>()
    .HasMany(project => project.Users)
    .WithMany(users => users.Projects)
    .UsingEntity<ProjectAssignment>();
  }
}