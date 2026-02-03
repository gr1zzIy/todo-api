using System.ComponentModel.DataAnnotations;
using todoApi.Domain.Enums;

namespace todoApi.Dtos;

public record UpdateTodoDto(
    [Required, MaxLength(200)] string Summary, 
    string Description,
    StatusItemType StatusId);