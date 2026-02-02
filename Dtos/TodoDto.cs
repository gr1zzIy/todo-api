using System.ComponentModel.DataAnnotations;

namespace todoApi.Dtos;

public sealed record TodoDto(
    Guid Id, 
    [Required, MaxLength(200)] string Title, 
    bool IsCompleted, 
    DateTimeOffset CreatedAt);