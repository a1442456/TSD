using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Http.Client;
using Cen.Wms.Domain.Sync.Api.Dtos;
using NodaTime;
using NodaTime.Extensions;
using Quartz;
using Serilog;

namespace Cen.Wms.Host.Sync.Jobs
{
    public class SyncPacsDownloadInvokeJob: IJob, IDisposable
    {
        private readonly IClock _clock;
        private readonly ILogger _logger;
        private readonly HttpQueryCall _httpQueryCall;
        private readonly SyncPacsDownloadInvokeJobOptions _syncPacsDownloadInvokeJobOptions;

        public SyncPacsDownloadInvokeJob(ILogger logger, HttpQueryCall httpQueryCall, SyncPacsDownloadInvokeJobOptions syncPacsDownloadInvokeJobOptions, IClock clock)
        {
            _clock = clock;
            _logger = logger;
            _httpQueryCall = httpQueryCall;
            _syncPacsDownloadInvokeJobOptions = syncPacsDownloadInvokeJobOptions;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();

            _logger.Information("SyncPacsDownloadInvokeJob: {0}, {1}", "Execute", _syncPacsDownloadInvokeJobOptions.Url);
            _httpQueryCall.RunRaw<SyncPacsDownloadReq, RpcResponse<SyncResp>>(
                new SyncPacsDownloadReq
                {
                    PacDateTimeFrom = _clock.InZone(tz).GetCurrentDate().PlusDays(-_syncPacsDownloadInvokeJobOptions.DaysBack).AtStartOfDayInZone(tz).ToInstant(),
                    PacDateTimeTo = _clock.InZone(tz).GetCurrentDate().PlusDays(_syncPacsDownloadInvokeJobOptions.DaysForward).AtStartOfDayInZone(tz).ToInstant(),
                },
                _syncPacsDownloadInvokeJobOptions.Url,
                _syncPacsDownloadInvokeJobOptions.TimeoutMs
            );
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.Information("SyncPacsDownloadInvokeJob: {0}", "Dispose");
        }
    }
}