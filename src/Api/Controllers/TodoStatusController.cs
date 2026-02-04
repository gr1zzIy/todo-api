using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using todoApi.Data;
using todoApi.Domain.Enums;
using todoApi.Domain.Extensions;
using todoApi.Domain.Helpers;
using todoApi.Dtos;

namespace todoApi.Controllers;

public class TodoStatusController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    
    public TodoStatusController(AppDbContext dbContext) => _dbContext = dbContext;

    [HttpGet("/by-status/{status}")]
    [SwaggerOperation(
        Summary = "Отримати задачі за статусом",
        Description = "Повертає список задач з вказаним статусом. Якщо таких немає — 204.",
        OperationId = "GetTodosByStatus"
    )]
    [ProducesResponseType(typeof(List<TodoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<List<TodoDto>>> GetByStatus([FromRoute] string status, CancellationToken ct)
    {
        var converterStatus = StatusHelper.GetStatusType(status);

        var items = await _dbContext.Todos
            .AsNoTracking()
            .Where(x => x.StatusId == converterStatus)
            .Select(x => new TodoDto(
                x.Id,
                x.Summary,
                x.Description,
                StatusHelper.GetStatusName(x.StatusId),
                x.CreatedAt,
                x.Deadline,
                x.EstimatedTimeMinutes,
                x.SpentTimeMinutes))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken: ct);
        
        return Ok(items);
    }
    
    [HttpPatch("{id:guid}/pending")]
    [SwaggerOperation(
        Summary = "Позначити задачу як очікуючу",
        Description = "Змінює статус задачі на 'Pending'. Якщо вже очікуюча — 204.",
        OperationId = "SetTodoPending"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Pending([FromRoute] Guid id, CancellationToken ct)
    {
        var item = await _dbContext.Todos
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (item is null) 
            return NoContent();

        if (item.IsPending())
            return NoContent();

        item.StatusId = StatusItemType.Pending;
        await _dbContext.SaveChangesAsync(ct);
        return Ok();
    }
    
    [HttpPatch("{id:guid}/in-progress")]
    [SwaggerOperation(
        Summary = "Позначити задачу як в процесі",
        Description = "Змінює статус задачі на 'InProgress'. Якщо вже в процесі — 204.",
        OperationId = "StartTodo"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> InProgress([FromRoute] Guid id, CancellationToken ct)
    {
        var item = await _dbContext.Todos
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (item is null) 
            return NoContent();

        if (item.IsInProgress())
            return NoContent();

        item.StatusId = StatusItemType.InProgress;
        await _dbContext.SaveChangesAsync(ct);
        return Ok();
    }

    [HttpPatch("{id:guid}/complete")]
    [SwaggerOperation(
        Summary = "Позначити задачу як виконану",
        Description = "Змінює статус задачі на 'Completed'. Якщо вже виконана — 204.",
        OperationId = "CompleteTodo"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Complete([FromRoute] Guid id, CancellationToken ct)
    {
        var item = await _dbContext.Todos
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        
        if (item is null) 
            return NoContent();

        if (item.IsCompleted())
            return NoContent();

        item.StatusId = StatusItemType.Completed;
        await _dbContext.SaveChangesAsync(ct);
        return Ok();
    }

    [HttpDelete("close-completed")]
    [SwaggerOperation(
        Summary = "Видалити виконані задачі",
        Description = "Видаляє всі задачі зі статусом 'Completed'. Якщо таких немає — 204.",
        OperationId = "DeleteCompletedTodos"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteCompleted(CancellationToken ct)
    {
        var completedTodos = await _dbContext.Todos
            .Where(t => t.StatusId == StatusItemType.Completed)
            .ToListAsync(ct);

        if (completedTodos.Count == 0)
            return NoContent();

        _dbContext.Todos.RemoveRange(completedTodos);
        await _dbContext.SaveChangesAsync(ct);
        return Ok();
    }
}