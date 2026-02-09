using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Data;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>(); // Аналог JPA Repository
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}