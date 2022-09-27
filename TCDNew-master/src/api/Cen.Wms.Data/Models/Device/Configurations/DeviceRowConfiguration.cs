using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Device.Configurations
{
    public class DeviceRowConfiguration: IEntityTypeConfiguration<DeviceRow>
    {
        public void Configure(EntityTypeBuilder<DeviceRow> builder)
        {
            builder.HasIndex(e => e.DevicePublicKey).IsUnique();
        }
    }
}