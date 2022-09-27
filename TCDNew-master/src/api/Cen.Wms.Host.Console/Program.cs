using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cen.Common.Configuration;
using Cen.Common.Http.Server;
using Cen.Common.Text;
using Cen.Wms.Domain.Auth.Api;
using Cen.Wms.Host.Sync;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Serilog;

namespace Cen.Wms.Host.Console
{
    public class Program
    {
        public static async Task Main()
        {
            var fileConfigSource = new FileConfigSource();
            fileConfigSource.SetupPgsqlWithNodaTime();
            var configurationRoot = fileConfigSource.GetCurrentConfiguration();
            Log.Logger = fileConfigSource.GetLoggerConfiguration(configurationRoot).CreateBootstrapLogger();

            await InitSecurityKeys(configurationRoot);
            
            IHost host = null;
            Exception exception = null;
            try
            {
                Log.Information("Web host: starting");
                var hostBuilderFactory = new HostBuilderFactory();
                var builder = hostBuilderFactory.CreateCoreHostBuilder<Startup>(
                    configurationRoot, 
                    fileConfigSource.GetCurrentDirectory()
                );
                builder.ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    serviceCollection.AddHostedService<SyncSchedulerHostedService>();
                });
                
                host = builder.Build();
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                exception = ex;
                Log.Fatal(ex, "Web host: terminating unexpectedly");
            }
            finally
            {
                Log.Information(exception == null ? "Web host: finishing" : "Web host: terminating");

                host?.Dispose();
                Log.CloseAndFlush();
            }
        }

        private static async Task InitSecurityKeys(IConfiguration configuration)
        {
            var authOptions = configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>();
            if (!File.Exists(authOptions.PrivateKeyFileName))
            {
                var ed25519KeyPairGenerator = new Ed25519KeyPairGenerator();
                ed25519KeyPairGenerator.Init(new Ed25519KeyGenerationParameters(SecureRandom.GetInstance("SHA256PRNG")));
                var asymmetricCipherKeyPair = ed25519KeyPairGenerator.GenerateKeyPair();
                var ed25519PrivateKeyParameters = (Ed25519PrivateKeyParameters) asymmetricCipherKeyPair.Private;
                var ed25519PublicKeyParameters = (Ed25519PublicKeyParameters) asymmetricCipherKeyPair.Public;

                var ed25519PrivateKey = ed25519PrivateKeyParameters.GetEncoded();
                await using var privateKeyStreamWriter = new StreamWriter(authOptions.PrivateKeyFileName, false, Encoding.UTF8);
                await privateKeyStreamWriter.WriteAsync(HexStringConverter.ByteArrayToHexString(ed25519PrivateKey));
                
                var ed25519PublicKey = ed25519PublicKeyParameters.GetEncoded();
                await using var publicKeyStreamWriter = new StreamWriter(authOptions.PublicKeyFileName, false, Encoding.UTF8);
                await publicKeyStreamWriter.WriteAsync(HexStringConverter.ByteArrayToHexString(ed25519PublicKey));
            }
        }
    }
}
