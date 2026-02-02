using Microsoft.EntityFrameworkCore;
using todoApi.Domain;

namespace todoApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<TodoItem> TodoItems { get; set;  } = null!;
}