using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.User.Configurations
{
    public class UserRowConfiguration: IEntityTypeConfiguration<UserRow>
    {
        public void Configure(EntityTypeBuilder<UserRow> builder)
        {
            builder.HasIndex(userRow => userRow.ExtId);
            builder.HasIndex(userRow => userRow.ChangedAt);
        }
    }
}