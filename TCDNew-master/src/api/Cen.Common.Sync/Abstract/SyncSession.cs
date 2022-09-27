using System.Threading.Tasks;
using Cen.Common.Sync.Interfaces;

namespace Cen.Common.Sync.Abstract
{
    public abstract class SyncSession: ISyncSession
    {
        private int? _sessionId;

        public int? SessionId => _sessionId;

        public async Task StoreSessionStart()
        {
            _sessionId = await StoreSessionStartInt();
        }

        public async Task StoreSessionFinish()
        {
            if (_sessionId.HasValue)
            {
                var sessionIdInternal = _sessionId.Value;
                _sessionId = null;
                await StoreSessionFinishInt(sessionIdInternal);
            }
        }

        public async Task<int> StoreSessionStepStart(string stepEntityName)
        {
            int result = 0;

            if (_sessionId.HasValue)
                result = await StoreSessionStepStartInt(_sessionId.Value, stepEntityName);

            return result;
        }

        public async Task StoreSessionStepTotal(int sessionStepId, long countTotal)
        {
            if (_sessionId.HasValue)
                await StoreSessionStepTotalInt(_sessionId.Value, sessionStepId, countTotal);
        }

        public async Task StoreSessionStepProgress(int sessionStepId, long countProcessed)
        {
            if (_sessionId.HasValue)
                await StoreSessionStepProgressInt(_sessionId.Value, sessionStepId, countProcessed);
        }

        public async Task StoreSessionStepFinish(int sessionStepId)
        {
            if (_sessionId.HasValue)
                await StoreSessionStepFinishInt(_sessionId.Value, sessionStepId);
        }

        protected abstract Task<int?> StoreSessionStartInt();

        protected abstract Task StoreSessionFinishInt(int sessionId);

        protected abstract Task<int> StoreSessionStepStartInt(int sessionId, string stepEntityName);

        protected abstract Task StoreSessionStepTotalInt(int sessionId, int sessionStepId, long countTotal);

        protected abstract Task StoreSessionStepProgressInt(int sessionId, int sessionStepId, long countProcessed);

        protected abstract Task StoreSessionStepFinishInt(int sessionId, int sessionStepId);
    }
}