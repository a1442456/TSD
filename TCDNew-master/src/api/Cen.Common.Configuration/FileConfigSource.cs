using System;
using Cen.Common.IO;
using Cen.Common.IO.Abstract;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace Cen.Common.Configuration
{
    public class FileConfigSource
    {
        public IConfigurationRoot GetCurrentConfiguration()
        {
            var currentDirectory = GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();

            return configuration;
        }

        public LoggerConfiguration GetLoggerConfiguration(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext();
        }
        
        public string GetCurrentDirectory()
        {
            // doesn't work in windows service
            ICurrentDirectoryProvider currentDirectoryProvider = new CurrentDirectoryProviderByIO();
            // doesn't work with migrations
            // var currentDirectoryProvider = new CurrentDirectoryProviderByAssembly();

            return currentDirectoryProvider.GetCurrentDirectory();
        }

        public void SetupPgsqlWithNodaTime()
        {
            NpgsqlConnection.GlobalTypeMapper.UseNodaTime();
        }
    }
}