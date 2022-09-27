using System.IO;
using SimpleInjector;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Http.Client;
using Cen.Common.Http.Server.CQRS;
using Cen.Common.Ioc;
using Cen.Common.Ioc.Npgsql;
using Cen.Common.Mapping;
using Cen.Common.Sync;
using Cen.Common.Sync.Interfaces;
using Cen.IdentityModel.EdDsa;
using Cen.Wms.Common;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Auth.Api;
using Cen.Wms.Domain.Auth.Providers.Fake;
using Cen.Wms.Domain.Auth.Providers.LsFusion;
// using Cen.Wms.Domain.Auth.Providers.LsFusion;
using Cen.Wms.Domain.Device.Manage.Api;
using Cen.Wms.Domain.Device.Manage.Store.EntityFramework;
using Cen.Wms.Domain.Facility.Access.Api;
using Cen.Wms.Domain.Facility.Access.Store.EntityFramework;
using Cen.Wms.Domain.Facility.Config.Api;
using Cen.Wms.Domain.Facility.Config.Store.EntityFramework;
using Cen.Wms.Domain.Facility.Manage.Api;
using Cen.Wms.Domain.Facility.Manage.Store.EntityFramework;
using Cen.Wms.Domain.Purchase.Api;
using Cen.Wms.Domain.Sync.Api;
using Cen.Wms.Domain.Sync.Providers.EntityFramework;
using Cen.Wms.Domain.Sync.Providers.Fake;
using Cen.Wms.Domain.Sync.Providers.Internal;
using Cen.Wms.Domain.Sync.Providers.LsFusion;
// using Cen.Wms.Domain.Sync.Providers.LsFusion;
using Cen.Wms.Domain.User.Config.Api;
using Cen.Wms.Domain.User.Config.Store.EntityFramework;
using Cen.Wms.Domain.User.Manage.Api;
using Cen.Wms.Domain.User.Manage.Store.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

namespace Cen.Wms.Host.Console
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly Container _container;
        private readonly HttpQueryRunner _httpQueryRunner;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;
            
            _container.UseNodaTime();
            _container.UseSerilog();
            _container.UseJsonSerializerOptions();

            var connectionString = configuration.GetConnectionString(ConstsApp.DbConnectionStringName);
            _container.UseNpgsql(connectionString);
            var optionsBuilder = new DbContextOptionsBuilder<WmsContext>();
            optionsBuilder.UseNpgsql(
                connectionString,
                x => x.UseNodaTime()
            );
            _container.RegisterInstance(optionsBuilder.Options);
            _container.Register<WmsContext>(Lifestyle.Scoped);
            _container.Register<UnitOfWork<WmsContext>>(Lifestyle.Scoped);

            _container.Register<HttpQueryCall>(Lifestyle.Singleton);
            _container.Register<ISyncPositionProvider, TimeBasedSyncPositionProvider>(Lifestyle.Singleton);

            DeviceManageModule.RegisterTypes(_container);
            DeviceManageStoreEntityFrameworkModule.RegisterTypes(_configuration, _container);
            AuthModule.RegisterTypes(_configuration, _container);
            AuthProvidersLsFusionModule.RegisterTypes(_configuration, _container);
            // AuthProvidersFakeModule.RegisterTypes(_container);
            FacilityManageModule.RegisterTypes(_container);
            UserManageModule.RegisterTypes(_container);
            UserManageStoreEntityFrameworkModule.RegisterTypes(_configuration, _container);
            UserConfigModule.RegisterTypes(_container);
            UserConfigStoreEntityFrameworkModule.RegisterTypes(_configuration, _container);
            FacilityManageStoreEntityFrameworkModule.RegisterTypes(_configuration, _container);
            FacilityAccessModule.RegisterTypes(_container);
            FacilityAccessStoreEntityFrameworkModule.RegisterTypes(_configuration, _container);
            FacilityConfigModule.RegisterTypes(_container);
            FacilityConfigStoreEntityFrameworkModule.RegisterTypes(_configuration, _container);
            PurchaseModule.RegisterTypes(_container);
            SyncProvidersEntityFrameworkModule.RegisterTypes(_container);
            // SyncProvidersFakeModule.RegisterTypes(configuration, _container);
            SyncProvidersLsFusionModule.RegisterTypes(_configuration, _container);
            SyncProvidersInternalModule.RegisterTypes(_configuration, _container);
            SyncModule.RegisterTypes(_container);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile<CommonProfile>();
                SyncProvidersInternalModule.RegisterProfiles(cfg);
                UserManageStoreEntityFrameworkModule.RegisterProfiles(cfg);
                UserConfigStoreEntityFrameworkModule.RegisterProfiles(cfg);
                FacilityManageStoreEntityFrameworkModule.RegisterProfiles(cfg);
                FacilityConfigStoreEntityFrameworkModule.RegisterProfiles(cfg);
                DeviceManageStoreEntityFrameworkModule.RegisterProfiles(cfg);
                PurchaseModule.RegisterProfiles(cfg);
            });
            _container.RegisterInstance(mapperConfiguration.CreateMapper());

            _container.Verify();
            _httpQueryRunner = new HttpQueryRunner(_container);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            ConfigureServicesAuth(services);
        }

        private void ConfigureServicesAuth(IServiceCollection services)
        {
            var authOptions = new AuthOptions();
            _configuration.GetSection(AuthOptions.SectionName).Bind(authOptions);
            
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.tokens.cryptoproviderfactory.customcryptoprovider?view=azure-dotnet
            CryptoProviderFactory.Default.CustomCryptoProvider = new EdDsaCryptoProvider();
            services
                .AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // укзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        // строка, представляющая издателя
                        ValidIssuer = authOptions.Issuer,
                        
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,
                        // будет ли валидироваться потребитель токена
                        ValidateAudience = false,
                        
                        // установка ключа безопасности
                        IssuerSigningKey = new Ed25519SecurityKey(authOptions.GetPublicKeyParameters()),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true
                    };
                });
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpointRouteBuilder =>
            {
                DeviceManageModule.RegisterRoutes(endpointRouteBuilder, _httpQueryRunner);
                AuthModule.RegisterRoutes(endpointRouteBuilder, _httpQueryRunner);
                UserManageModule.RegisterRoutes(endpointRouteBuilder, _httpQueryRunner);
                UserConfigModule.RegisterRoutes(endpointRouteBuilder, _httpQueryRunner);
                FacilityManageModule.RegisterRoutes(endpointRouteBuilder, _httpQueryRunner);
                FacilityAccessModule.RegisterRoutes(endpointRouteBuilder, _httpQueryRunner);
                FacilityConfigModule.RegisterRoutes(endpointRouteBuilder, _httpQueryRunner);
                PurchaseModule.RegisterRoutes(endpointRouteBuilder, _httpQueryRunner);
                SyncModule.RegisterRoutes(endpointRouteBuilder, _httpQueryRunner);
            });
            
            var appOptions = new AppOptions();
            _configuration.GetSection(AppOptions.SectionName).Bind(appOptions);
                
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings[".apk"] = "application/vnd";
            var fileProvider = new PhysicalFileProvider(
                Path.GetFullPath(appOptions.StaticFolderPath)
            );
            app.UseDefaultFiles(new DefaultFilesOptions {FileProvider = fileProvider});
            app.UseStaticFiles(new StaticFileOptions {FileProvider = fileProvider, ContentTypeProvider = contentTypeProvider});
        }
    }
}