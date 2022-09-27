using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NodaTime.Serialization.SystemTextJson.Custom
{
    public class NullableInstantConverter: JsonConverter<Nullable<Instant>>
    {
        public override Instant? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetInt64(out var milliseconds))
            {
                return Instant.FromUnixTimeMilliseconds(milliseconds);
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, Instant? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value.ToUnixTimeMilliseconds());
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}