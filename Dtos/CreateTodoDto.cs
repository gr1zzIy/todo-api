using System.ComponentModel.DataAnnotations;

namespace todoApi.Dtos;

public record CreateTodoDto(
    [Required, MaxLength(200)] string Summary,
    [MaxLength(254)] string? Description
    );