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
    public class LsFusionPacExtSource: ISyncSource<ReqPacInterval, PacExt>
    {
        private readonly IClock _clock;
        private readonly HttpQueryCall _httpQueryCall;
        private readonly SyncProvidersLsFusionOptions _syncProvidersLsFusionOptions;
        private readonly string _basicAuthenticationHeaderEncodedValue;
        
        public LsFusionPacExtSource(SyncProvidersLsFusionOptions syncProvidersLsFusionOptions, IClock clock, HttpQueryCall httpQueryCall)
        {
            _clock = clock;
            _httpQueryCall = httpQueryCall;
            _syncProvidersLsFusionOptions = syncProvidersLsFusionOptions;
            _basicAuthenticationHeaderEncodedValue = BasicAuthHelper.GetBasicAuthHeaderString(
                _syncProvidersLsFusionOptions.WMSServiceBaseLogin,
                _syncProvidersLsFusionOptions.WMSServiceBasePassword
            );
        }
        
        public async Task<long> Count(ISyncPositionsStore positionsStore, string stepEntityName, ReqPacInterval syncParameter)
        {
            long result = 0;
            try
            {
                var rpcResponseExtIds = await _httpQueryCall.RunRaw<LsFusionReqPacInterval, List<LsFusionPacExtId>>(
                    new LsFusionReqPacInterval(syncParameter.PacDateTimeFrom, syncParameter.PacDateTimeTo),
                    $"{_syncProvidersLsFusionOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.pac_ids_list",
                    _syncProvidersLsFusionOptions.TimeoutMs,
                    _basicAuthenticationHeaderEncodedValue
                );

                if (rpcResponseExtIds != null)
                    result = rpcResponseExtIds.Count;
            }
            catch (Exception)
            {
                //
            }

            return result;
        }
        
        public async IAsyncEnumerable<PacExt> AsEnumerable(ISyncPositionsStore positionsStore, string stepEntityName, ReqPacInterval syncParameter)
        {
            var rpcResponseExtIds = await _httpQueryCall.RunRaw<LsFusionReqPacInterval, List<LsFusionPacExtId>>(
                new LsFusionReqPacInterval(syncParameter.PacDateTimeFrom, syncParameter.PacDateTimeTo),
                $"{_syncProvidersLsFusionOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.pac_ids_list",
                _syncProvidersLsFusionOptions.TimeoutMs,
                _basicAuthenticationHeaderEncodedValue
            );
                
            rpcResponseExtIds = rpcResponseExtIds.ToList();
            
            var idsBatch = new List<LsFusionPacExtId>();
            using var rpcResponseExtIdsEnumerator = rpcResponseExtIds.GetEnumerator();
            var itemAcquired = false;
            do
            {
                itemAcquired = rpcResponseExtIdsEnumerator.MoveNext();
                if (itemAcquired)
                    idsBatch.Add(rpcResponseExtIdsEnumerator.Current);

                if ((itemAcquired && idsBatch.Count >= _syncProvidersLsFusionOptions.BatchSize) || (!itemAcquired && idsBatch.Count > 0))
                {
                    var request = idsBatch.Select(e => new LsFusionByIdReq {Id = e.PacId}).ToArray();
                    idsBatch.Clear();
                    
                    var rpcResponseExtItems = await _httpQueryCall.RunRaw<LsFusionByIdReq[], List<LsFusionPacExt>>(
                        request,
                        $"{_syncProvidersLsFusionOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.pac_read",
                        _syncProvidersLsFusionOptions.TimeoutMs,
                        _basicAuthenticationHeaderEncodedValue
                    );

                    if (rpcResponseExtItems == null)
                        continue;

                    foreach (var rpcResponseExtItem in rpcResponseExtItems)
                    {
                        yield return new PacExt
                        {
                            PacId = rpcResponseExtItem.PacId,
                            PacDateTime = rpcResponseExtItem.PacDateTime,
                            FacilityId = rpcResponseExtItem.FacilityId,
                            SupplierId = rpcResponseExtItem.SupplierId,
                            SupplierName = rpcResponseExtItem.SupplierName,
                            PurchaseBookingId = rpcResponseExtItem.PurchaseBookingId,
                            PurchaseBookingDate = rpcResponseExtItem.PurchaseBookingDate,
                            PurchaseId = rpcResponseExtItem.PurchaseId,
                            PurchaseDate = rpcResponseExtItem.PurchaseDate,
                            Lines = rpcResponseExtItem.Lines.Select(e => new PacLineExt
                            {
                                LineNum = e.LineNum,
                                PacLineId = e.PacLineId,
                                ProductId = e.ProductId,
                                ProductName = e.ProductName,
                                ProductAbc = e.ProductAbc,
                                ProductBarcodeMain = e.ProductBarcodeMain,
                                ProductUnitOfMeasure = e.ProductUnitOfMeasure,
                                QtyExpected = e.QtyExpected,
                                ProductBarcodes = e.ProductBarcodes
                            }).ToList(),
                            ChangedAt = rpcResponseExtItem.ChangedAt
                        };
                    }
                }
            } while (itemAcquired);
        }
    }
}