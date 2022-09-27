using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cen.Wms.Client.Utils
{
    public static class JsonHelpers
    {
        public static byte[] Serialize(object value)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsonSerializerSettings.Converters.Add(
                new UnixDateTimeConverter()
            );
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                value, 
                Formatting.None,
                jsonSerializerSettings
            ));
        }

        public static void Serialize(Stream s, object value)
        {
            var writer = new StreamWriter(s);
            var jsonWriter = new JsonTextWriter(writer);
            var ser = new JsonSerializer();
            ser.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ser.Converters.Add(
                new UnixDateTimeConverter()
            );
            ser.Serialize(jsonWriter, value);
            jsonWriter.Flush();
        }

        public static T Deserialize<T>(byte[] value)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsonSerializerSettings.Converters.Add(
                new UnixDateTimeConverter()
            );
            return JsonConvert.DeserializeObject<T>(
                Encoding.UTF8.GetString(value, 0, value.Length),
                jsonSerializerSettings
            );
        }

        public static T Deserialize<T>(Stream s)
        {
            var reader = new StreamReader(s);
            var jsonReader = new JsonTextReader(reader);
            var ser = new JsonSerializer();
            ser.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ser.Converters.Add(
                new UnixDateTimeConverter()
            );
            return ser.Deserialize<T>(jsonReader);
        }
    }
}
