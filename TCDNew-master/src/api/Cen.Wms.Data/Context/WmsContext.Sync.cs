using Cen.Wms.Data.Models.Sync;
using Cen.Wms.Data.Models.Sync.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Data.Context
{
    public partial class WmsContext
    {
        public DbSet<SyncPositionRow> SyncPosition { get; set; }
        
        public void SyncPositionApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SyncPositionRowConfiguration());
        }        
    }
}