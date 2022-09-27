using System.Threading;
using System.Threading.Tasks;
using Cen.Wms.Host.Sync.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace Cen.Wms.Host.Sync
{
    public class SyncSchedulerHostedService: IHostedService
    {
        private readonly IConfiguration _configuration;
        private IScheduler _scheduler;

        public SyncSchedulerHostedService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("Sync scheduler: starting");
            
            var syncSchedulerHostedServiceOptions = _configuration
                .GetSection(SyncSchedulerHostedServiceOptions.SectionName)
                .Get<SyncSchedulerHostedServiceOptions>();
            
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler(cancellationToken);
            _scheduler.JobFactory = new SimpleInjectorJobFactory(_configuration);
            
            await ScheduleSyncCatalogsDownloadInvokeJob(syncSchedulerHostedServiceOptions, cancellationToken);
            await ScheduleSyncPacsDownloadInvokeJob(syncSchedulerHostedServiceOptions, cancellationToken);
            await ScheduleSyncPacsUploadInvokeJob(syncSchedulerHostedServiceOptions, cancellationToken);

            await _scheduler.Start(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("Sync scheduler: stopping");
            
            return _scheduler.Shutdown(cancellationToken);
        }

        private async Task ScheduleSyncCatalogsDownloadInvokeJob(SyncSchedulerHostedServiceOptions syncSchedulerHostedServiceOptions, CancellationToken cancellationToken)
        {
            if (syncSchedulerHostedServiceOptions.SyncCatalogsDownloadInvokeJobEnabled)
            {
                var echoJob = new JobDetailImpl("SyncCatalogsDownloadInvokeJob", typeof(SyncCatalogsDownloadInvokeJob));
                var echoJobTrigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithDescription("SyncCatalogsDownloadInvokeJobTrigger")
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(syncSchedulerHostedServiceOptions.SyncCatalogsDownloadInvokeJobIntervalInSeconds)
                        .RepeatForever())
                    .Build();
            
                await _scheduler.ScheduleJob(echoJob, echoJobTrigger, cancellationToken);
            }
        }
        
        private async Task ScheduleSyncPacsDownloadInvokeJob(SyncSchedulerHostedServiceOptions syncSchedulerHostedServiceOptions, CancellationToken cancellationToken)
        {
            if (syncSchedulerHostedServiceOptions.SyncPacsDownloadInvokeJobEnabled)
            {
                var echoJob = new JobDetailImpl("SyncPacsDownloadInvokeJob", typeof(SyncPacsDownloadInvokeJob));
                var echoJobTrigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithDescription("SyncPacsDownloadInvokeJobTrigger")
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(syncSchedulerHostedServiceOptions.SyncPacsDownloadInvokeJobIntervalInSeconds)
                        .RepeatForever())
                    .Build();
            
                await _scheduler.ScheduleJob(echoJob, echoJobTrigger, cancellationToken);
            }
        }
        
        private async Task ScheduleSyncPacsUploadInvokeJob(SyncSchedulerHostedServiceOptions syncSchedulerHostedServiceOptions, CancellationToken cancellationToken)
        {
            if (syncSchedulerHostedServiceOptions.SyncPacsUploadInvokeJobEnabled)
            {
                var echoJob = new JobDetailImpl("SyncPacsUploadInvokeJob", typeof(SyncPacsUploadInvokeJob));
                var echoJobTrigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithDescription("SyncPacsUploadInvokeJobTrigger")
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(syncSchedulerHostedServiceOptions.SyncPacsUploadInvokeJobIntervalInSeconds)
                        .RepeatForever())
                    .Build();
            
                await _scheduler.ScheduleJob(echoJob, echoJobTrigger, cancellationToken);
            }
        }
    }
}