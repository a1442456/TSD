using System.Threading.Tasks;
using Cen.Common.Sync.Abstract;
using Serilog;

namespace Cen.Wms.Domain.Sync.Providers.EntityFramework.Infrastructures
{
    public class EntityFrameworkSyncSession: SyncSession
    {
        private readonly ILogger _logger;

        public EntityFrameworkSyncSession(ILogger logger)
        {
            _logger = logger;
        }
        
        protected override async Task<int?> StoreSessionStartInt()
        {
            _logger.Information($"StoreSessionStartInt");
            return 365;
        }

        protected override async Task StoreSessionFinishInt(int sessionId)
        {
            _logger.Information($"StoreSessionFinishInt: {sessionId}");
        }

        protected override async Task<int> StoreSessionStepStartInt(int sessionId, string stepEntityName)
        {
            _logger.Information($"StoreSessionStepStartInt: {sessionId}, {stepEntityName}");
            return 375;
        }

        protected override async Task StoreSessionStepTotalInt(int sessionId, int sessionStepId, long countTotal)
        {
            _logger.Information($"StoreSessionStepTotalInt: {sessionId}, {sessionStepId}, {countTotal}");
        }

        protected override async Task StoreSessionStepProgressInt(int sessionId, int sessionStepId, long countProcessed)
        {
            _logger.Information($"StoreSessionStepProgressInt: {sessionId}, {sessionStepId}, {countProcessed}");
        }

        protected override async Task StoreSessionStepFinishInt(int sessionId, int sessionStepId)
        {
            _logger.Information($"StoreSessionStepFinishInt: {sessionId}, {sessionStepId}");
        }
    }
}