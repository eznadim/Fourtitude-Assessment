using System.Text.Json;

namespace TransactionAPI.Utils;

public static class LoggingHelper
{
    public static string FormatRequestResponse(string type, object data)
    {
        return $"{type}:\n{JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true })}";
    }
}