using System.Threading.Tasks;

namespace Cen.Common.Sync.Interfaces
{
    public interface ISyncSession
    {
        int? SessionId { get; }
        Task StoreSessionStart();
        Task StoreSessionFinish();
        
        Task<int> StoreSessionStepStart(string stepEntityName);
        Task StoreSessionStepTotal(int sessionStepId, long countTotal);
        Task StoreSessionStepProgress(int sessionStepId, long countProcessed);
        Task StoreSessionStepFinish(int sessionStepId);
    }
}