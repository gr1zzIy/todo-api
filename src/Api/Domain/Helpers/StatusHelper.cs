using todoApi.Domain.Enums;

namespace todoApi.Domain.Helpers;

public class StatusHelper
{
    private static readonly Dictionary<StatusItemType, string> _statusNames = new()
    {
        { StatusItemType.Pending, "Pending" },
        { StatusItemType.InProgress, "In Progress" },
        { StatusItemType.Completed, "Completed" }
    };
    
    private static readonly Dictionary<string, StatusItemType> _reverseMap = 
        _statusNames.ToDictionary(
            kvp => kvp.Value.Replace(" ", "", StringComparison.OrdinalIgnoreCase),
            kvp => kvp.Key,
            StringComparer.OrdinalIgnoreCase
        );
    
    public static string GetStatusName(StatusItemType status)
    {
        return _statusNames.GetValueOrDefault(status, "Unknown Status");
    }

    public static StatusItemType GetStatusType(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status cannot be null or empty.", nameof(status));
        
        var cleanedStatus = status.Trim();
        
        if (int.TryParse(cleanedStatus, out int intValue) && 
            Enum.IsDefined(typeof(StatusItemType), intValue))
        {
            return (StatusItemType)intValue;
        }
        
        if (Enum.TryParse<StatusItemType>(cleanedStatus, true, out var enumResult))
        {
            return enumResult;
        }
        
        var key = cleanedStatus.Replace(" ", "", StringComparison.OrdinalIgnoreCase);
        if (_reverseMap.TryGetValue(key, out var result))
        {
            return result;
        }
        
        throw new ArgumentException($"Invalid status string: '{status}'");
    }
}