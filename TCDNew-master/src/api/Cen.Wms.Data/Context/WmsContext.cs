using Cen.Common.Data.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Data.Context
{
    public partial class WmsContext: DbContext
    {
        private readonly DbContextOptions<WmsContext> _options;
        
        public WmsContext(DbContextOptions<WmsContext> options)
            : base(options)
        {
            _options = options;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.UseSnakeCase();
            modelBuilder.UseXminAsConcurrencyToken();

            UserApplyConfigurations(modelBuilder);
            DeviceApplyConfigurations(modelBuilder);
            FacilityApplyConfigurations(modelBuilder);
            PurchaseApplyConfigurations(modelBuilder);
            SyncPositionApplyConfigurations(modelBuilder);
        }
    }
}