using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Cen.Common.Http.Server
{
    public class HostBuilderFactory
    {
        public IHostBuilder CreateCoreHostBuilder<TStartup>(IConfigurationRoot configurationRoot, string currentDirectory) where TStartup : class
        {
            var pathToContentRoot = Directory.GetParent(currentDirectory).FullName;
            
            return 
                new HostBuilder()
                    .UseSerilog()
                    .ConfigureWebHost(webHostBuilder =>
                    {
                        webHostBuilder.CaptureStartupErrors(true);
                        webHostBuilder
                            .UseConfiguration(configurationRoot)
                            .UseContentRoot(pathToContentRoot)
                            .UseKestrel(options => options.AddServerHeader = false)
                            .UseStartup<TStartup>();
                    });
        }
    }
}