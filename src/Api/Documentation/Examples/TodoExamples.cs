using Swashbuckle.AspNetCore.Filters;
using todoApi.Dtos;
using todoApi.Domain.Enums;

namespace todoApi.Documentation.Examples;

public sealed class TodoListResponseExample : IExamplesProvider<List<TodoDto>>
{
    public List<TodoDto> GetExamples() => new()
    {
        new TodoDto(
            Id: Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            Summary: "Купити продукти",
            Description: "Молоко, яйця, хліб",
            Status: "Pending",
            CreatedAt: DateTimeOffset.Parse("2024-01-15T10:30:00Z")
        )
    };
}

public sealed class CreateTodoRequestExample : IExamplesProvider<CreateTodoDto>
{
    public CreateTodoDto GetExamples() => new(
        Summary: "Купити продукти",
        Description: "Молоко, яйця, хліб, овочі"
    );
}

public sealed class UpdateTodoRequestExample : IExamplesProvider<UpdateTodoDto>
{
    public UpdateTodoDto GetExamples() => new(
        Summary: "Оновлена задача",
        Description: "Оновлений опис задачі",
        StatusId: StatusItemType.InProgress
    );
}

public sealed class TodoResponseExample : IExamplesProvider<TodoDto>
{
    public TodoDto GetExamples() => new(
        Id: Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
        Summary: "Приклад задачі",
        Description: "Опис прикладної задачі",
        Status: "Pending",
        CreatedAt: DateTimeOffset.Parse("2024-01-15T10:30:00Z")
    );
}