namespace TaskTrackerAPI.DTOs;

public record TaskResponseDto(int Id, string Title, string? Description, bool IsCompleted, DateTime CreatedAt);