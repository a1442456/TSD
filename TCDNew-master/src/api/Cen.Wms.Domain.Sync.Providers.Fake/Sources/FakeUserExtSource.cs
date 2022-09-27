using System.Collections.Generic;
using System.Threading.Tasks;
using Cen.Common.Sync.Interfaces;
using Cen.Wms.Domain.Sync.Models;
using NodaTime;

namespace Cen.Wms.Domain.Sync.Providers.Fake.Sources
{
    public class FakeUserExtSource: ISyncSource<object, UserExt>
    {
        private readonly IClock _clock;
        private readonly SyncProvidersFakeOptions _syncProvidersFakeOptions;
        private readonly IList<UserExt> _users;

        public FakeUserExtSource(SyncProvidersFakeOptions syncProvidersFakeOptions, IClock clock)
        {
            _clock = clock;
            _syncProvidersFakeOptions = syncProvidersFakeOptions;
            _users = new List<UserExt>();
            for (var i = 0; i < _syncProvidersFakeOptions.UsersCount; i++)
                _users.Add(
                    new UserExt
                    {
                        UserId = $"ID{i}", 
                        UserName = $"USER{i}", 
                        UserLogin = $"user_{i}", 
                        IsLocked = false,
                        ChangedAt = _clock.GetCurrentInstant()
                    }
                );
        }
        
        public async Task<long> Count(ISyncPositionsStore positionsStore, string stepEntityName, object syncParameter)
        {
            var latestPositionLong = await positionsStore.GetPosition(stepEntityName);
            var latestPositionInstant = Instant.FromUnixTimeMilliseconds(latestPositionLong);
            
            return _users.Count;
        }

        public async IAsyncEnumerable<UserExt> AsEnumerable(ISyncPositionsStore positionsStore, string stepEntityName, object syncParameter)
        {
            var latestPositionLong = await positionsStore.GetPosition(stepEntityName);
            var latestPositionInstant = Instant.FromUnixTimeMilliseconds(latestPositionLong);
            
            foreach (var facilityExt in _users)
            {
                yield return facilityExt;
            }
        }
    }
}