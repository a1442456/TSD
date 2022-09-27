using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cen.Common.Http.Client;
using Cen.Common.Sync.Interfaces;
using Cen.Wms.Domain.Sync.Models;
using Cen.Wms.Domain.Sync.Providers.LsFusion.Dtos;
using NodaTime;

namespace Cen.Wms.Domain.Sync.Providers.LsFusion.Sources
{
    public class LsFusionUserExtSource: ISyncSource<object, UserExt>
    {
        private readonly IClock _clock;
        private readonly HttpQueryCall _httpQueryCall;
        private readonly SyncProvidersLsFusionOptions _syncProvidersLsFusionOptions;
        private readonly string _basicAuthenticationHeaderEncodedValue;

        public LsFusionUserExtSource(SyncProvidersLsFusionOptions syncProvidersLsFusionOptions, IClock clock, HttpQueryCall httpQueryCall)
        {
            _clock = clock;
            _httpQueryCall = httpQueryCall;
            _syncProvidersLsFusionOptions = syncProvidersLsFusionOptions;
            _basicAuthenticationHeaderEncodedValue = BasicAuthHelper.GetBasicAuthHeaderString(
                _syncProvidersLsFusionOptions.WMSServiceBaseLogin,
                _syncProvidersLsFusionOptions.WMSServiceBasePassword
            );
        }
        
        public async Task<long> Count(ISyncPositionsStore positionsStore, string stepEntityName, object syncParameter)
        {
            long result = 0;
            try
            {
                var latestPositionLong = await positionsStore.GetPosition(stepEntityName);
                var latestPositionInstant = Instant.FromUnixTimeMilliseconds(latestPositionLong);
                
                var rpcResponseExtIds = await _httpQueryCall.RunRaw<object, List<LsFusionFacilityExtId>>(
                    null,
                    $"{_syncProvidersLsFusionOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.user_ids_list",
                    _syncProvidersLsFusionOptions.TimeoutMs,
                    _basicAuthenticationHeaderEncodedValue
                );

                if (rpcResponseExtIds != null)
                    result = rpcResponseExtIds.Count(e => e.ChangedAt.CompareTo(latestPositionInstant) > 0);
            }
            catch (Exception)
            {
                //
            }

            return result;
        }

        public async IAsyncEnumerable<UserExt> AsEnumerable(ISyncPositionsStore positionsStore, string stepEntityName, object syncParameter)
        {
            var latestPositionLong = await positionsStore.GetPosition(stepEntityName);
            var latestPositionInstant = Instant.FromUnixTimeMilliseconds(latestPositionLong);
                
            var rpcResponseExtIds = await _httpQueryCall.RunRaw<object, List<LsFusionUserExtId>>(
                null,
                $"{_syncProvidersLsFusionOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.user_ids_list",
                _syncProvidersLsFusionOptions.TimeoutMs,
                _basicAuthenticationHeaderEncodedValue
            );
            
            rpcResponseExtIds = rpcResponseExtIds.Where(e => e.ChangedAt.CompareTo(latestPositionInstant) > 0).ToList();

            var idsBatch = new List<LsFusionUserExtId>();
            using var rpcResponseExtIdsEnumerator = rpcResponseExtIds.GetEnumerator();
            var itemAcquired = false;
            do
            {
                itemAcquired = rpcResponseExtIdsEnumerator.MoveNext();
                if (itemAcquired)
                    idsBatch.Add(rpcResponseExtIdsEnumerator.Current);

                if ((itemAcquired && idsBatch.Count >= _syncProvidersLsFusionOptions.BatchSize) || (!itemAcquired && idsBatch.Count > 0))
                {
                    var request = idsBatch.Select(e => new LsFusionByIdReq {Id = e.UserId}).ToArray();
                    idsBatch.Clear();
                    var rpcResponseExtItems = await _httpQueryCall.RunRaw<LsFusionByIdReq[], List<LsFusionUserExt>>(
                        request,
                        $"{_syncProvidersLsFusionOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.user_read",
                        _syncProvidersLsFusionOptions.TimeoutMs,
                        _basicAuthenticationHeaderEncodedValue
                    );

                    if (rpcResponseExtItems == null)
                        continue;

                    foreach (var rpcResponseExtItem in rpcResponseExtItems)
                    {
                        yield return new UserExt
                        {
                            UserId = rpcResponseExtItem.UserId,
                            UserName = rpcResponseExtItem.UserName,
                            UserLogin = rpcResponseExtItem.UserLogin,
                            IsLocked = rpcResponseExtItem.IsLocked > 0,
                            FacilityId = rpcResponseExtItem.FacilityId, 
                            FacilityName = rpcResponseExtItem.FacilityName,
                            DepartmentId = rpcResponseExtItem.DepartmentId,
                            DepartmentName = rpcResponseExtItem.DepartmentName,
                            PositionId = rpcResponseExtItem.PositionId,
                            PositionName = rpcResponseExtItem.PositionName,
                            ChangedAt = rpcResponseExtItem.ChangedAt
                        };
                    }
                }
            } while (itemAcquired);
        }
    }
}