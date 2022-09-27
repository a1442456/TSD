using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Device.Configurations
{
    public class DeviceRegistrationRequestStateRowConfiguration: IEntityTypeConfiguration<DeviceRegistrationRequestStateRow>
    {
        public void Configure(EntityTypeBuilder<DeviceRegistrationRequestStateRow> builder)
        {
            
        }
    }
}