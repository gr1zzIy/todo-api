using System.ComponentModel.DataAnnotations;

namespace todoApi.Domain;

public sealed class TodoItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public bool IsCompleted { get; set; } = false;
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}