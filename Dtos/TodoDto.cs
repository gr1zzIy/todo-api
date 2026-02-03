using System.ComponentModel.DataAnnotations;

namespace todoApi.Dtos;

public sealed record TodoDto(
    Guid Id, 
    [Required, MaxLength(200)] string Summary, 
    [MaxLength(254)] string Description,
    bool IsCompleted, 
    DateTimeOffset CreatedAt);