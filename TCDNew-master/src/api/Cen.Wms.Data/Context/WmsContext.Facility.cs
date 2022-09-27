using Cen.Wms.Data.Models.Facility;
using Cen.Wms.Data.Models.Facility.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Data.Context
{
    public partial class WmsContext
    {
        public DbSet<FacilityRow> Facility { get; set; }
        public DbSet<FacilityConfigRow> FacilityConfig { get; set; }
        public DbSet<FacilityAccessRow> FacilityAccess { get; set; }
        
        public void FacilityApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FacilityRowConfiguration());
            modelBuilder.ApplyConfiguration(new FacilityConfigRowConfiguration());
            modelBuilder.ApplyConfiguration(new FacilityAccessRowConfiguration());
        }
    }
}