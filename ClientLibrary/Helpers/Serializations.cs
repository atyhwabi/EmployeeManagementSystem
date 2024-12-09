using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ClientLibrary.Helpers
{
    public class Serializations
    {
        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        public static IList<T> Deserialize<T>(byte[] json)
        {
            return json == null ? new List<T>() : JsonSerializer.Deserialize<IList<T>>(Encoding.UTF8.GetString(json));
        }
    }
}
