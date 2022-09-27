using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Http.Client;
using Cen.Wms.Domain.Sync.Api.Dtos;
using NodaTime;
using Quartz;
using Serilog;

namespace Cen.Wms.Host.Sync.Jobs
{
    public class SyncPacsUploadInvokeJob: IJob, IDisposable
    {
        private readonly IClock _clock;
        private readonly ILogger _logger;
        private readonly HttpQueryCall _httpQueryCall;
        private readonly SyncPacsUploadInvokeJobOptions _syncPacsUploadInvokeJobOptions;

        public SyncPacsUploadInvokeJob(ILogger logger, HttpQueryCall httpQueryCall, SyncPacsUploadInvokeJobOptions syncPacsUploadInvokeJobOptions, IClock clock)
        {
            _clock = clock;
            _logger = logger;
            _httpQueryCall = httpQueryCall;
            _syncPacsUploadInvokeJobOptions = syncPacsUploadInvokeJobOptions;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();

            _logger.Information("SyncPacsUploadInvokeJob: {0}, {1}", "Execute", _syncPacsUploadInvokeJobOptions.Url);
            _httpQueryCall.RunRaw<SyncPacsUploadReq, RpcResponse<SyncResp>>(
                new SyncPacsUploadReq(),
                _syncPacsUploadInvokeJobOptions.Url,
                _syncPacsUploadInvokeJobOptions.TimeoutMs
            );
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.Information("SyncPacsUploadInvokeJob: {0}", "Dispose");
        }
    }
}