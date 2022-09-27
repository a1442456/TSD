using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Device.Configurations
{
    public class DeviceStateRowConfiguration: IEntityTypeConfiguration<DeviceStateRow>
    {
        public void Configure(EntityTypeBuilder<DeviceStateRow> builder)
        {
            builder.HasIndex(e => e.DevicePublicKey).IsUnique();
        }
    }
}