using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.User.Configurations
{
    public class UserConfigRowConfiguration: IEntityTypeConfiguration<UserConfigRow>
    {
        public void Configure(EntityTypeBuilder<UserConfigRow> builder)
        {
            
        }
    }
}