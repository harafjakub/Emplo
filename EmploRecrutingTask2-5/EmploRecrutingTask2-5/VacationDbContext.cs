using EmploRecrutingTask2_5.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploRecrutingTask2_5;

public class VacationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<VacationPackage> VacationPackages { get; set; } = null!;
    public DbSet<Vacation> Vacations { get; set; } = null!;

    public VacationDbContext()
    {
    }

    public VacationDbContext(DbContextOptions<VacationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=vacation.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name);
            
            entity.HasOne(e => e.Team)
                .WithMany(t => t.Employees)
                .HasForeignKey(e => e.TeamId);
                  
            entity.HasOne(e => e.VacationPackage)
                .WithMany(vp => vp.Employees)
                .HasForeignKey(e => e.VacationPackageId);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name);
        });

        modelBuilder.Entity<VacationPackage>(entity =>
        {
            entity.HasKey(vp => vp.Id);
            entity.Property(vp => vp.Name);
        });

        modelBuilder.Entity<Vacation>(entity =>
        {
            entity.HasKey(v => v.Id);
            
            entity.HasOne(v => v.Employee)
                .WithMany(e => e.Vacations)
                .HasForeignKey(v => v.EmployeeId);
        });
    }
}
