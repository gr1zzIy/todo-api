using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using todoApi.Data;
using todoApi.Documentation.Examples;
using todoApi.Domain.Entities;
using todoApi.Domain.Enums;
using todoApi.Domain.Extensions;
using todoApi.Domain.Helpers;
using todoApi.Dtos;

namespace todoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[SwaggerTag("Задачі (Todos)")]
public sealed class TodosController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public TodosController(AppDbContext dbContext) => _dbContext = dbContext;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Отримати всі задачі",
        Description = "Повертає список всіх задач, відсортований за датою створення (новіші першими).",
        OperationId = "GetAllTodos"
    )]
    [ProducesResponseType(typeof(List<TodoDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TodoListResponseExample))]
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
                x.CreatedAt,
                x.Deadline,
                x.EstimatedTimeMinutes,
                x.SpentTimeMinutes))
            .ToListAsync(ct);

        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Отримати задачу за ID",
        Description = "Повертає задачу за вказаним GUID. Якщо не знайдено —204.",
        OperationId = "GetTodoById"
    )]
    [ProducesResponseType(typeof(TodoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TodoResponseExample))]
    public async Task<ActionResult<TodoDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var item = await _dbContext.Todos
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TodoDto(
                x.Id,
                x.Summary,
                x.Description,
                StatusHelper.GetStatusName(x.StatusId),
                x.CreatedAt,
                x.Deadline,
                x.EstimatedTimeMinutes,
                x.SpentTimeMinutes))
            .FirstOrDefaultAsync(ct);

        return item is null ? NoContent() : Ok(item);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Створити нову задачу",
        Description = "Додає нову задачу до системи зі статусом 'Pending'. Поле Summary є обов'язковим.",
        OperationId = "CreateTodo"
    )]
    [ProducesResponseType(typeof(TodoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(CreateTodoDto), typeof(CreateTodoRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(TodoResponseExample))]
    public async Task<ActionResult<TodoDto>> Create(
        [FromBody, SwaggerRequestBody("Дані нової задачі", Required = true)] CreateTodoDto dto,
        CancellationToken ct)
    {
        var summary = (dto.Summary ?? string.Empty).Trim();
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
            entity.CreatedAt,
            entity.Deadline,
            entity.EstimatedTimeMinutes,
            entity.SpentTimeMinutes);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Повністю оновити задачу",
        Description = "Оновлює всі поля існуючої задачі. Поле Summary є обов'язковим.",
        OperationId = "UpdateTodo"
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(UpdateTodoDto), typeof(UpdateTodoRequestExample))]
    public async Task<ActionResult> Update(
        [FromRoute] Guid id,
        [FromBody, SwaggerRequestBody("Оновлені дані задачі", Required = true)] UpdateTodoDto dto,
        CancellationToken ct)
    {
        var summary = (dto.Summary ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(summary))
            return BadRequest(new { error = "Summary is required." });

        var item = await _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (item is null) 
            return NoContent();

        item.Summary = summary;
        item.Description = dto.Description?.Trim() ?? string.Empty;
        item.StatusId = dto.StatusId;

        await _dbContext.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Видалити задачу",
        Description = "Видаляє задачу за GUID. Якщо не знайдено — 204.",
        OperationId = "DeleteTodo"
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        var item = await _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (item is null) 
            return NoContent();

        _dbContext.Todos.Remove(item);
        await _dbContext.SaveChangesAsync(ct);
        return NoContent();
    }
}