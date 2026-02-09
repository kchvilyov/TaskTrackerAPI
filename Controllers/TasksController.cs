using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            // Возвращает все задачи из БД
            return await _context.Tasks.ToListAsync();
        }

        // GET api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound(); // Вернёт статус 404
            }
            return task;
        }

        // POST api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Стандартная практика: вернуть созданный объект и код 201 Created
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskItem updatedTask)
        {
            if (id != updatedTask.Id)
            {
                return BadRequest(); // ID в пути и в теле не совпадают
            }

            _context.Entry(updatedTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tasks.Any(t => t.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent(); // Стандартный ответ для успешного PUT - 204 No Content
        }

        // DELETE api/tasks/5
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