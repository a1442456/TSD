using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjector;
using Cen.Common.Http.Client;
using Cen.Common.Ioc;
using Cen.Wms.Host.Sync.Jobs;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Spi;
using SimpleInjector.Lifestyles;

namespace Cen.Wms.Host.Sync
{
    public class SimpleInjectorJobFactory: IJobFactory
    {
        private readonly IConfiguration _configuration;
        private readonly Container _container;
        private readonly Dictionary<Type, InstanceProducer> _jobProducers;

        public SimpleInjectorJobFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            
            _container.UseNodaTime();
            _container.UseSerilog();
            _container.UseJsonSerializerOptions();

            _container.Register<HttpQueryCall>(Lifestyle.Singleton);
            _container.Register(
                () => _configuration.GetSection(SyncCatalogsDownloadInvokeJobOptions.SectionName).Get<SyncCatalogsDownloadInvokeJobOptions>(),
                Lifestyle.Scoped
            );
            // _container.Register<SyncCatalogsDownloadInvokeJob>(Lifestyle.Scoped);
            _container.Register(
                () => _configuration.GetSection(SyncPacsDownloadInvokeJobOptions.SectionName).Get<SyncPacsDownloadInvokeJobOptions>(),
                Lifestyle.Scoped
            );
            // _container.Register<SyncPacsDownloadInvokeJob>(Lifestyle.Scoped);
            _container.Register(
                () => _configuration.GetSection(SyncPacsUploadInvokeJobOptions.SectionName).Get<SyncPacsUploadInvokeJobOptions>(),
                Lifestyle.Scoped
            );
            // _container.Register<SyncPacsUploadInvokeJob>(Lifestyle.Scoped);
            // _container.RegisterDecorator(typeof(IJob), typeof(SimpleInjectorJobDecorator));

            // By creating producers, jobs can be decorated.
            var transient = Lifestyle.Transient;
            _jobProducers =
                _container.GetTypesToRegister(typeof(IJob), Assembly.GetExecutingAssembly()).ToDictionary(
                    type => type,
                    type => transient.CreateProducer(typeof(IJob), type, _container)
                );
            
            _container.Verify(VerificationOption.VerifyOnly);
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobProducer = _jobProducers[bundle.JobDetail.JobType];
                    return new SimpleInjectorJobDecorator(
                        _container, () => (IJob)jobProducer.GetInstance());
        }

        public void ReturnJob(IJob job)
        {
            // This will be handled automatically by Simple Injector
        }
    }
}