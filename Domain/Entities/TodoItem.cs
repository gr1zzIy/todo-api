using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using todoApi.Domain.Enums;

namespace todoApi.Domain.Entities;

public sealed class TodoItem
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(200)]
    public string Summary { get; set; } = string.Empty;
    
    [MaxLength(254)]
    public string Description { get; set; } = string.Empty;
    
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    
    [ForeignKey(nameof(StatusItem))]
    public StatusItemType StatusId { get; set; } = StatusItemType.Pending;
    
    public StatusItem? Status { get; set; }
}