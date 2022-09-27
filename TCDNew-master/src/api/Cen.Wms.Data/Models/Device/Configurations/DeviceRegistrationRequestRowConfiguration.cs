using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Device.Configurations
{
    public class DeviceRegistrationRequestRowConfiguration: IEntityTypeConfiguration<DeviceRegistrationRequestRow>
    {
        public void Configure(EntityTypeBuilder<DeviceRegistrationRequestRow> builder)
        {
            builder.HasIndex(e => e.DevicePublicKey).IsUnique();
            builder.HasIndex(e => new {e.DevicePublicKey, e.CreatedAt}).IsUnique();
        }
    }
}