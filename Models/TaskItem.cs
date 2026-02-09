namespace TaskTrackerAPI.Models;

public class TaskItem
{
    public int Id { get; set; }          // Аналог 'var id: Int' в Kotlin data class
    public string Title { get; set; }    // Not-null по умолчанию (как в Kotlin)
    public string? Description { get; set; } // Nullable (аналог 'String?')
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}

// В Kotlin для сравнения:
// data class TaskItem(
//     val id: Int,
//     val title: String,
//     val description: String?,
//     val isCompleted: Boolean,
//     val createdAt: LocalDateTime
// )