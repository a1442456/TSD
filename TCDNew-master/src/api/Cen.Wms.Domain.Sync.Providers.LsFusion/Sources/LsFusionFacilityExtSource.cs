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
    public class LsFusionFacilityExtSource: ISyncSource<object, FacilityExt>
    {
        private readonly IClock _clock;
        private readonly HttpQueryCall _httpQueryCall;
        private readonly SyncProvidersLsFusionOptions _syncProvidersLsFusionOptions;
        private readonly string _basicAuthenticationHeaderEncodedValue;

        public LsFusionFacilityExtSource(SyncProvidersLsFusionOptions syncProvidersLsFusionOptions, IClock clock, HttpQueryCall httpQueryCall)
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
                    $"{_syncProvidersLsFusionOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.facility_ids_list",
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

        public async IAsyncEnumerable<FacilityExt> AsEnumerable(ISyncPositionsStore positionsStore, string stepEntityName, object syncParameter)
        {
            var latestPositionLong = await positionsStore.GetPosition(stepEntityName);
            var latestPositionInstant = Instant.FromUnixTimeMilliseconds(latestPositionLong);
                
            var rpcResponseExtIds = await _httpQueryCall.RunRaw<object, List<LsFusionFacilityExtId>>(
                null,
                $"{_syncProvidersLsFusionOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.facility_ids_list",
                _syncProvidersLsFusionOptions.TimeoutMs,
                _basicAuthenticationHeaderEncodedValue
            );
                
            rpcResponseExtIds = rpcResponseExtIds.Where(e => e.ChangedAt.CompareTo(latestPositionInstant) > 0).ToList();
            
            var idsBatch = new List<LsFusionFacilityExtId>();
            using var rpcResponseExtIdsEnumerator = rpcResponseExtIds.GetEnumerator();
            var itemAcquired = false;
            do
            {
                itemAcquired = rpcResponseExtIdsEnumerator.MoveNext();
                if (itemAcquired)
                    idsBatch.Add(rpcResponseExtIdsEnumerator.Current);

                if ((itemAcquired && idsBatch.Count >= _syncProvidersLsFusionOptions.BatchSize) || (!itemAcquired && idsBatch.Count > 0))
                {
                    var request = idsBatch.Select(e => new LsFusionByIdReq {Id = e.FacilityId}).ToArray();
                    idsBatch.Clear();
                    
                    var rpcResponseExtItems = await _httpQueryCall.RunRaw<LsFusionByIdReq[], List<LsFusionFacilityExt>>(
                        request,
                        $"{_syncProvidersLsFusionOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.facility_read",
                        _syncProvidersLsFusionOptions.TimeoutMs,
                        _basicAuthenticationHeaderEncodedValue
                    );

                    if (rpcResponseExtItems == null)
                        continue;

                    foreach (var rpcResponseExtItem in rpcResponseExtItems)
                    {
                        yield return new FacilityExt
                        {
                            FacilityId = rpcResponseExtItem.FacilityId, 
                            FacilityName = rpcResponseExtItem.FacilityName,
                            ChangedAt = rpcResponseExtItem.ChangedAt
                        };
                    }
                }
            } while (itemAcquired);
        }
    }
}