using Cen.Wms.Data.Models.Device;
using Cen.Wms.Data.Models.Device.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Data.Context
{
    public partial class WmsContext
    {
        public DbSet<DeviceRow> Device { get; set; }
        public DbSet<DeviceStateRow> DeviceState { get; set; }
        public DbSet<DeviceRegistrationRequestRow> DeviceRegistrationRequest { get; set; }
        public DbSet<DeviceRegistrationRequestStateRow> DeviceRegistrationRequestState { get; set; }
        
        public void DeviceApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DeviceRowConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceStateRowConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceRegistrationRequestRowConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceRegistrationRequestStateRowConfiguration());
        }
    }
}