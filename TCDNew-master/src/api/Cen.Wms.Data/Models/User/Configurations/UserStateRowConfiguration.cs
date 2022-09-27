using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.User.Configurations
{
    public class UserStateRowConfiguration: IEntityTypeConfiguration<UserStateRow>
    {
        public void Configure(EntityTypeBuilder<UserStateRow> builder)
        {
            
        }
    }
}