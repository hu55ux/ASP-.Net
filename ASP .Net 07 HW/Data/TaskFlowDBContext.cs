using ASP_.Net_07_HW.Models;
using Microsoft.EntityFrameworkCore;
namespace ASP_.Net_07_HW.Data;

public class TaskFlowDBContext : DbContext
{
    public TaskFlowDBContext(DbContextOptions options) : base(options)
    { }
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Project>(
            project =>
            {
                project.HasKey(p => p.Id);
                project.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                project.Property(p => p.Description)
                    .IsRequired()
                    .HasMaxLength(1000);
                project.Property(p => p.CreatedAt)
                    .IsRequired();
            }
            );

        modelBuilder.Entity<TaskItem>(
            task =>
            {
                task.HasKey(t => t.Id);
                task.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                task.Property(t => t.Description)
                    .IsRequired()
                    .HasMaxLength(1000);
                task.Property(t => t.CreatedAt)
                    .IsRequired();
                task.Property(t => t.Status)
                    .IsRequired();

                task.HasOne(t => t.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(t => t.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
            );
    }
}
