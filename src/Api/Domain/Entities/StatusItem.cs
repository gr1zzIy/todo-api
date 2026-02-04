using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using todoApi.Domain.Enums;

namespace todoApi.Domain.Entities;

public sealed class StatusItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public StatusItemType Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    
    public StatusItem(StatusItemType id, string name)
    {
        Id = id;
        Name = name;
    }
    
    private StatusItem() { }
    
    public static IEnumerable<StatusItem> GetAll()
    {
        return
        [
            new StatusItem(StatusItemType.Pending, "Pending"),
            new StatusItem(StatusItemType.InProgress, "In Progress"),
            new StatusItem(StatusItemType.Completed, "Completed")
        ];
    }
}