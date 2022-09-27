using System.Threading.Tasks;
using Cen.Common.Sync.Interfaces;

namespace Cen.Common.Sync
{
    public class FakeSyncPositionProvider: ISyncPositionProvider
    {
        private readonly long _syncPosition;
        
        public FakeSyncPositionProvider(long syncPosition)
        {
            _syncPosition = syncPosition;
        }
        
        public async Task<long> GetCurrentPosition(string stepEntityName)
        {
            return _syncPosition;
        }
    }
}