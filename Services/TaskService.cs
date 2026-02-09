using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Services;

public class TaskService
{
    private readonly AppDbContext _context;
    
    public TaskService(AppDbContext context) // DI через конструктор (как в Spring)
    {
        _context = context;
    }
    
    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        // LINQ запрос - аналог Stream API в Java/Kotlin, но более декларативный
        return await _context.Tasks
            .Where(t => !t.IsCompleted)   // Аналог .filter { !it.isCompleted }
            .OrderBy(t => t.CreatedAt)    // Аналог .sortedBy { it.createdAt }
            .ToListAsync();
    }
}