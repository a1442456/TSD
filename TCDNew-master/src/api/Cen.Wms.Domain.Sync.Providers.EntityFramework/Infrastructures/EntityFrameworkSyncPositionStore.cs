using System.Threading.Tasks;
using Cen.Common.Sync.Interfaces;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Sync;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.Sync.Providers.EntityFramework.Infrastructures
{
    public class EntityFrameworkSyncPositionStore: ISyncPositionsStore
    {
        private readonly WmsContext _wmsContext;

        public EntityFrameworkSyncPositionStore(WmsContext wmsContext)
        {
            _wmsContext = wmsContext;
        }
        
        public async Task<long> GetPosition(string stepEntityName)
        {
            var syncPositionRow = await _wmsContext.SyncPosition.FirstOrDefaultAsync(e => e.Name == stepEntityName);
            return syncPositionRow?.Position ?? 0;
        }

        public async Task SetPosition(string stepEntityName, long currentPosition)
        {
            var syncPositionRow = await _wmsContext.SyncPosition.FirstOrDefaultAsync(e => e.Name == stepEntityName);
            if (syncPositionRow == null)
            {
                syncPositionRow = new SyncPositionRow { Id = NewId.NextGuid(), Name = stepEntityName, Position = currentPosition };
                await _wmsContext.SyncPosition.AddAsync(syncPositionRow);
            }
            else
            {
                syncPositionRow.Position = currentPosition;
                _wmsContext.SyncPosition.Update(syncPositionRow);
            }
        }
    }
}