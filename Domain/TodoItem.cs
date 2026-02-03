using System.ComponentModel.DataAnnotations;

namespace todoApi.Domain;

public sealed class TodoItem
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(200)]
    public string Summary { get; set; } = string.Empty;
    
    [MaxLength(254)]
    public string Description { get; set; } = string.Empty;
    
    public bool IsCompleted { get; set; } = false;
    
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}