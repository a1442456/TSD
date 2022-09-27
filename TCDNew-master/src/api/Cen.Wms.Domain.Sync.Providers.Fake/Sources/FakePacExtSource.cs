using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cen.Common.Sync.Interfaces;
using Cen.Wms.Domain.Sync.Models;
using NodaTime;

namespace Cen.Wms.Domain.Sync.Providers.Fake.Sources
{
    public class FakePacExtSource: ISyncSource<ReqPacInterval, PacExt>
    {
        private readonly IClock _clock;
        private readonly SyncProvidersFakeOptions _syncProvidersFakeOptions;
        private List<PacExt> _pacs;

        public FakePacExtSource(SyncProvidersFakeOptions syncProvidersFakeOptions, IClock clock)
        {
            _clock = clock;
            _syncProvidersFakeOptions = syncProvidersFakeOptions;
            _pacs = new List<PacExt>(_syncProvidersFakeOptions.PacsCount);

            for (var i = 0; i < _syncProvidersFakeOptions.PacsCount; i++)
            {
                var lines = new List<PacLineExt>();
                var random = new Random();
                for (var j = 0; j < 3 + random.Next(1, 3); j++)
                {
                    var barcodes = Enumerable
                        .Range(0, 3)
                        .Select(k => (1000 + j * 10 + k).ToString())
                        .ToList();
                    
                    lines.Add(new PacLineExt
                    {
                        LineNum = j,
                        PacLineId = $"СКЛП00{j}",
                        ProductId = $"ПРД00{j}",
                        ProductName = $"Продукт {j}",
                        ProductAbc = "a",
                        ProductBarcodeMain = (10000 + j * 10).ToString(),
                        ProductUnitOfMeasure = "шт.",
                        QtyExpected = Convert.ToDecimal(j + 1),
                        ProductBarcodes = barcodes
                    });
                }
                
                _pacs.Add(
                    new PacExt
                    {
                        PacId = $"КЛП00{i}",
                        PacDateTime = _clock.GetCurrentInstant(),
                        FacilityId = "БОР",
                        SupplierId = $"ПОСТ00{i}",
                        SupplierName = $"Поставщик {i}",
                        PurchaseBookingId = $"ЗКЗ00{i}",
                        PurchaseBookingDate = _clock.GetCurrentInstant().InUtc().LocalDateTime.Date.Minus(Period.FromDays(3)),
                        PurchaseId = $"ЗКП00{i}",
                        PurchaseDate = _clock.GetCurrentInstant().InUtc().LocalDateTime.Date,
                        Lines = lines,
                        ChangedAt = _clock.GetCurrentInstant().Minus(Duration.FromDays(1))
                    });
            }
        }
        
        public async Task<long> Count(ISyncPositionsStore positionsStore, string stepEntityName, ReqPacInterval syncParameter)
        {
            var latestPositionLong = await positionsStore.GetPosition(stepEntityName);
            var latestPositionInstant = Instant.FromUnixTimeMilliseconds(latestPositionLong);
            return _pacs.Count;
        }
    
        public async IAsyncEnumerable<PacExt> AsEnumerable(ISyncPositionsStore positionsStore, string stepEntityName, ReqPacInterval syncParameter)
        {
            var latestPositionLong = await positionsStore.GetPosition(stepEntityName);
            var latestPositionInstant = Instant.FromUnixTimeMilliseconds(latestPositionLong);
            foreach (var pacExt in _pacs)
            {
                yield return pacExt;
            }
        }
    }
}