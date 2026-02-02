using System.ComponentModel.DataAnnotations;

namespace todoApi.Dtos;

public record UpdateTodoDto(
    [Required, MaxLength(200)] string Title, 
    bool IsCompleted);