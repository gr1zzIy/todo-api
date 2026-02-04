using System.ComponentModel.DataAnnotations;
using todoApi.Domain.Enums;

namespace todoApi.Dtos;

public sealed record TodoDto(
    Guid Id, 
    [Required, MaxLength(200)] string Summary, 
    [MaxLength(254)] string Description,
    string Status, 
    DateTimeOffset CreatedAt,
    DateTimeOffset? Deadline,
    int? EstimatedTimeMinutes,
    int SpentTimeMinutes);