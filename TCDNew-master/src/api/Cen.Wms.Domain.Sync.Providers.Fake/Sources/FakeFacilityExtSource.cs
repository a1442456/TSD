using System.Collections.Generic;
using System.Threading.Tasks;
using Cen.Common.Sync.Interfaces;
using Cen.Wms.Domain.Sync.Models;
using NodaTime;

namespace Cen.Wms.Domain.Sync.Providers.Fake.Sources
{
    public class FakeFacilityExtSource: ISyncSource<object, FacilityExt>
    {
        private readonly IClock _clock;
        private readonly SyncProvidersFakeOptions _syncProvidersFakeOptions;
        private readonly IList<FacilityExt> _facilities; 
        
        public FakeFacilityExtSource(SyncProvidersFakeOptions syncProvidersFakeOptions, IClock clock)
        {
            _clock = clock;
            _syncProvidersFakeOptions = syncProvidersFakeOptions;
            _facilities = new List<FacilityExt>();
            for (var i = 0; i < _syncProvidersFakeOptions.FacilitiesCount; i++)
                _facilities.Add(
                    new FacilityExt
                    {
                        FacilityId = i.ToString(),
                        FacilityName = i.ToString(),
                        ChangedAt = _clock.GetCurrentInstant()
                    });
        }
        
        public async Task<long> Count(ISyncPositionsStore positionsStore, string stepEntityName, object syncParameter)
        {
            var latestPositionLong = await positionsStore.GetPosition(stepEntityName);
            var latestPositionInstant = Instant.FromUnixTimeMilliseconds(latestPositionLong);
            return _facilities.Count;
        }

        public async IAsyncEnumerable<FacilityExt> AsEnumerable(ISyncPositionsStore positionsStore, string stepEntityName, object syncParameter)
        {
            var latestPositionLong = await positionsStore.GetPosition(stepEntityName);
            var latestPositionInstant = Instant.FromUnixTimeMilliseconds(latestPositionLong);
            foreach (var facilityExt in _facilities)
            {
                yield return facilityExt;
            }
        }
    }
}