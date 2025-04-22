using System.Text.Json;

namespace GameSharedLib.Utils;

public static class SerializationHelper
{
    public static string ToJson<T>(T obj) =>
        JsonSerializer.Serialize(obj);

    public static T? FromJson<T>(string json) =>
        JsonSerializer.Deserialize<T>(json);
}
