using System;
using CustomJson.MiniJSON;

public class JsonParse
{
    public static T DeserializeObject<T>(string jsonString) where T : class {
        return Json.Deserialize(jsonString) as T;
    }

    public static string SerializeObject(object value) {
        return Json.Serialize(value);
    }
}