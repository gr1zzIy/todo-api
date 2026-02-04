using todoApi.Domain.Entities;
using todoApi.Domain.Enums;

namespace todoApi.Domain.Extensions;

public static class TodoItemExtensions
{
    public static bool IsPending(this TodoItem todo) 
        => todo.StatusId == StatusItemType.Pending;
    
    public static bool IsInProgress(this TodoItem todo) 
        => todo.StatusId == StatusItemType.InProgress;
    
    public static bool IsCompleted(this TodoItem todo) 
        => todo.StatusId == StatusItemType.Completed;
    
    public static void MarkAsPending(this TodoItem todo) 
        => todo.StatusId = StatusItemType.Pending;
    
    public static void MarkAsInProgress(this TodoItem todo)
        => todo.StatusId = StatusItemType.InProgress;
    
    public static void MarkAsCompleted(this TodoItem todo)
        => todo.StatusId = StatusItemType.Completed;

    public static bool IsOverdue(this TodoItem todo)
    {
        if (!todo.Deadline.HasValue)
            return false;
            
        if (todo.IsCompleted())
            return false;
            
        return DateTimeOffset.UtcNow > todo.Deadline.Value;
    }
    
    public static bool IsTimeExceeded(this TodoItem todo)
    {
        if (!todo.EstimatedTimeMinutes.HasValue)
            return false;
            
        return todo.SpentTimeMinutes > todo.EstimatedTimeMinutes.Value;
    }
}