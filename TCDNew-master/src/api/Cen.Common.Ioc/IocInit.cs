using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using SimpleInjector;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using NodaTime.Serialization.SystemTextJson.Custom;
using Serilog;

namespace Cen.Common.Ioc
{
    public static class IocInit
    {
        public static void UseNodaTime(this Container container)
        {
            container
                .RegisterInstance<IClock>(SystemClock.Instance);
        }

        public static void UseSerilog(this Container container)
        {
            container
                .RegisterInstance<ILogger>(Log.Logger);
        }

        public static void UseJsonSerializerOptions(this Container container)
        {
            container
                .RegisterInstance<JsonSerializerOptions>(GetJsonSerializerOptions());
        }
        
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            jsonSerializerOptions.Converters.Add(new InstantConverterFactory());
            jsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            return jsonSerializerOptions;
        }
    }
}