namespace Blog.Backend.Utils;

using System.Text.Json;
using Newtonsoft.Json.Linq;

public static class JsonUtil
{
    public static T? Deserialize<T>(string json, bool ignoreRoot) where T : class
    {
        return ignoreRoot 
            ? JObject.Parse(json)?.Properties()?.First()?.Value?.ToObject<T>()
            : JObject.Parse(json)?.ToObject<T>();
    }
}