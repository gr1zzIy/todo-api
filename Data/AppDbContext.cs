using Microsoft.EntityFrameworkCore;
using todoApi.Domain;
using todoApi.Domain.Entities;

namespace todoApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<TodoItem> Todos => Set<TodoItem>();
    
    public DbSet<StatusItem> Statuses => Set<StatusItem>();

    // Налаштування моделі даних EF Core | TodoItem та StatusItem
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Summary).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(254);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("getutcdate()");
            
            // Зв'язок з StatusItem
            entity.HasOne(e => e.Status)
                .WithMany(s => s.TodoItems)
                .HasForeignKey(e => e.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StatusItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            
            entity.Property(e => e.Id)
                .ValueGeneratedNever();
            
            // Початкові дані для статусів
            entity.HasData(StatusItem.GetAll());
        });
    }
}