using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NodaTime.Serialization.SystemTextJson.Custom
{
    public class InstantConverterFactory: JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert == typeof(Instant))
                return true;

            if (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Nullable<>) && typeToConvert.GenericTypeArguments[0] == typeof(Instant))
                return true;

            return false;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            JsonConverter result = null;
            
            if (typeToConvert == typeof(Instant))
                result = new InstantConverter();

            if (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Nullable<>) && typeToConvert.GenericTypeArguments[0] == typeof(Instant))
                result = new NullableInstantConverter();

            return result;
        }
    }
}