using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.DTOs;

namespace TaskTrackerAPI.Controllers
{
    // Аналог @RequestMapping в Spring
    [Route("api/[controller]")] // Маршрут будет: /api/tasks
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
        {
            var tasks = await _context.Tasks
                .Select(t => new TaskResponseDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsCompleted,
                    t.CreatedAt))
                .ToListAsync();

            return Ok(tasks);
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
        {
            var task = await _context.Tasks
                .Where(t => t.Id == id)
                .Select(t => new TaskResponseDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsCompleted,
                    t.CreatedAt))
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return NotFound(); // Вернёт статус 404
            }
            return task;
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> PostTask(TaskCreateDto dto)
        {
            var task = new Models.TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Стандартная практика: вернуть созданный объект и код 201 Created
            var responseDto = new TaskResponseDto(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.CreatedAt);

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, responseDto);
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskUpdateDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            // CreatedAt не изменяется

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent(); // Стандартный ответ для успешного DELETE - 204 No Content
        }
    }
}