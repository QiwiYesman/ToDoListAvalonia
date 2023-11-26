using System.IO;
using Newtonsoft.Json;

namespace ToDoTree.Models;
public static class Serializer
{
    public static void SerializeToFileJson(object obj, string filePath)
    {
        using var fileWriter = new StreamWriter(filePath);
        var serializer = new JsonSerializer{ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
        serializer.Serialize(fileWriter, obj);
    }
    
    public static T? DeserializeFromFileJson<T>(string filePath) where T : class
    {
        using var fileReader = new StreamReader(filePath);
        var serializer = new JsonSerializer(){ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
        return serializer.Deserialize(fileReader, typeof(T)) as T;
    }
}