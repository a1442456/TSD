using System.Threading.Tasks;
using Cen.Common.Sync.Interfaces;
using NodaTime;

namespace Cen.Common.Sync
{
    public class TimeBasedSyncPositionProvider: ISyncPositionProvider
    {
        private readonly IClock _clock;

        public TimeBasedSyncPositionProvider(IClock clock)
        {
            _clock = clock;
        }
        
        public async Task<long> GetCurrentPosition(string stepEntityName)
        {
            return _clock.GetCurrentInstant().ToUnixTimeMilliseconds();
        }
    }
}