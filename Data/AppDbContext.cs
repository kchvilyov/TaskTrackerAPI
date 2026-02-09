using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Data;

using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>(); // Аналог JPA Repository
}