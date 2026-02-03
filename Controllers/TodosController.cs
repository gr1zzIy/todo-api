using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoApi.Data;
using todoApi.Domain.Helpers;
using todoApi.Domain.Entities;
using todoApi.Domain.Extensions;
using todoApi.Domain.Enums;
using todoApi.Dtos;

namespace todoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class TodosController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public TodosController(AppDbContext dbContext) => _dbContext = dbContext;

    /// <summary>Отримати список задач.</summary>
    /// <remarks>Повертає задачі відсортовані за CreatedAt (новіші першими).</remarks>
    [HttpGet]
    [ProducesResponseType(typeof(List<TodoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TodoDto>>> GetAll(CancellationToken ct)
    {
        var items = await _dbContext.Todos
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Select(x => new TodoDto(
                x.Id, 
                x.Summary, 
                x.Description, 
                StatusHelper.GetStatusName(x.StatusId), 
                x.CreatedAt))
            .ToListAsync(ct);

        return Ok(items);
    }

    /// <summary>Отримати задачу за ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TodoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoDto>> GetById(Guid id, CancellationToken ct)
    {
        var item = await _dbContext.Todos
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TodoDto(
                x.Id, 
                x.Summary, 
                x.Description, 
                StatusHelper.GetStatusName(x.StatusId), 
                x.CreatedAt))
            .FirstOrDefaultAsync(ct);

        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>Створити нову задачу.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TodoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoDto>> Create(CreateTodoDto dto, CancellationToken ct)
    {
        var summary = dto.Summary.Trim();
        if (string.IsNullOrWhiteSpace(summary))
            return BadRequest(new { error = "Summary is required." });

        var entity = new TodoItem
        {
            Summary = summary,
            Description = dto.Description?.Trim() ?? string.Empty,
            StatusId = StatusItemType.Pending,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _dbContext.Todos.Add(entity);
        await _dbContext.SaveChangesAsync(ct);

        var result = new TodoDto(
            entity.Id,
            entity.Summary,
            entity.Description,
            StatusHelper.GetStatusName(entity.StatusId),
            entity.CreatedAt);
        
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
    }

    /// <summary>Оновити задачу повністю.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(Guid id, UpdateTodoDto dto, CancellationToken ct)
    {
        var summary = dto.Summary.Trim();
        if (string.IsNullOrWhiteSpace(summary))
            return BadRequest(new { error = "Summary is required." });

        var item = await _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (item is null) return NotFound();

        item.Summary = summary;
        item.Description = dto.Description?.Trim() ?? string.Empty;
        item.StatusId = dto.StatusId;

        await _dbContext.SaveChangesAsync(ct);
        return NoContent();
    }

    /// <summary>Видалити задачу.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
    {
        var item = await _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (item is null) return NotFound();

        _dbContext.Todos.Remove(item);
        await _dbContext.SaveChangesAsync(ct);
        return NoContent();
    }
    
    // <summary>Виконати задачу.</summary>
    [HttpPatch("{id:guid}/Complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ToggleCompletionStatus(Guid id, CancellationToken ct)
    {
        var item = await _dbContext.Todos
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (item is null)
            return NotFound();
        
        if (item.IsCompleted())
            return NoContent();

        item.StatusId = StatusItemType.Completed;
        await _dbContext.SaveChangesAsync(ct);
        return NoContent();
    }
    
    // <summary>Видалити виконані задачі.</summary>
    [HttpDelete("Completed")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteCompletedTodos(CancellationToken ct)
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
