using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Hein.Helpers;

public static class TempDataHelper
{
    public static void SetTempData(Controller controller, string key, string value, bool keep = true)
    {
        controller.TempData[key] = value;

        if (keep)
        {
            controller.TempData.Keep(key);
        }
    }

    public static string GetTempData(Controller controller, string key, bool keep = true)
    {
        var value = controller.TempData[key] as string;

        if (keep)
        {
            controller.TempData.Keep(key);
        }

        return value;
    }

    public static void RemoveTempData(Controller controller, string key)
    {
        controller.TempData.Remove(key);
    }

    public static void KeepAllTempData(Controller controller)
    {
        foreach (var key in controller.TempData.Keys)
        {
            controller.TempData.Keep(key);
        }
    }

    // New: Serialize and store an object in TempData
    public static void SetObject<T>(Controller controller, string key, T value, bool keep = true) where T : class
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        // Serialize object to JSON and store it
        controller.TempData[key] = JsonSerializer.Serialize(value);

        if (keep)
        {
            controller.TempData.Keep(key);
        }
    }

    // Retrieve and deserialize an object from TempData
    public static T GetObject<T>(Controller controller, string key, bool keep = true) where T : class
    {
        if (controller.TempData.TryGetValue(key, out var tempDataValue) && tempDataValue is string jsonString)
        {
            try
            {
                var value = JsonSerializer.Deserialize<T>(jsonString);
                if (keep)
                {
                    controller.TempData.Keep(key);
                }
                return value;
            }
            catch (JsonException)
            {
                // Handle deserialization error if needed
                return null;
            }
        }

        return null;
    }
}
