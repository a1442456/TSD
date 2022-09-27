using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Http.Client;
using Cen.Wms.Domain.Sync.Api.Dtos;
using Quartz;
using Serilog;

namespace Cen.Wms.Host.Sync.Jobs
{
    public class SyncCatalogsDownloadInvokeJob: IJob, IDisposable
    {
        private readonly ILogger _logger;
        private readonly HttpQueryCall _httpQueryCall;
        private readonly SyncCatalogsDownloadInvokeJobOptions _syncCatalogsDownloadInvokeJobOptions;

        public SyncCatalogsDownloadInvokeJob(ILogger logger, HttpQueryCall httpQueryCall, SyncCatalogsDownloadInvokeJobOptions syncCatalogsDownloadInvokeJobOptions)
        {
            _logger = logger;
            _httpQueryCall = httpQueryCall;
            _syncCatalogsDownloadInvokeJobOptions = syncCatalogsDownloadInvokeJobOptions;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.Information("SyncCatalogsDownloadInvokeJob: {0}, {1}", "Execute", _syncCatalogsDownloadInvokeJobOptions.Url);
            _httpQueryCall.RunRaw<SyncCatalogsReq, RpcResponse<SyncResp>>(new SyncCatalogsReq(),
                _syncCatalogsDownloadInvokeJobOptions.Url,
                _syncCatalogsDownloadInvokeJobOptions.TimeoutMs
            );
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.Information("SyncCatalogsDownloadInvokeJob: {0}", "Dispose");
        }
    }
}