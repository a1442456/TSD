using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cen.Wms.Client.Utils
{
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var seconds = Convert.ToInt64(reader.Value);
            if (seconds == 0)
            {
                return null;
            }

            object result = null;
            if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
            {
                result = UnixEpoch.AddMilliseconds(seconds);
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long? seconds = null;
            if (value is DateTime)
            {
                var dtValue = (value as DateTime?);
                seconds = Convert.ToInt64((dtValue.Value.ToUniversalTime() - UnixEpoch).TotalMilliseconds);
                if (seconds == 0)
                {
                    seconds = null;
                }
            }

            writer.WriteValue(seconds);
        }
    }
}
