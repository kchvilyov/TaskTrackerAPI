namespace TaskTrackerAPI.DTOs;

public record TaskUpdateDto(string Title, string? Description, bool IsCompleted);