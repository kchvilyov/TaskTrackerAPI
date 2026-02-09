using Microsoft.AspNetCore.Mvc;
using TaskTrackerAPI.Services;

namespace TaskTrackerAPI.Controllers;

[Microsoft.AspNetCore.Components.Route("api/[controller]")] // Аналог @RequestMapping в Spring
public class TasksController : ControllerBase
{
    private readonly TaskService _service;
    
    public TasksController(TaskService service) => _service = service;
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _service.GetAllTasksAsync();
        return Ok(tasks); // Автоматическая сериализация в JSON (как @RestController)
    }
}