using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PacHeadRowConfiguration: IEntityTypeConfiguration<PacHeadRow>
    {
        public void Configure(EntityTypeBuilder<PacHeadRow> builder)
        {
            builder.HasIndex(pacHeadRow => pacHeadRow.ExtId);
            builder.HasIndex(pacHeadRow => pacHeadRow.ChangedAt);
            builder.HasIndex(pacHeadRow => pacHeadRow.PacDateTime);
            
            builder
                .HasMany(pacHeadRow => pacHeadRow.Lines)
                .WithOne(pacLineRow => pacLineRow.PacHead);
            
            builder
                .HasOne(pacHeadRow => pacHeadRow.PacState)
                .WithOne(pacStateRow => pacStateRow.PacHead);
            
            builder
                .HasOne(pacHeadRow => pacHeadRow.ResponsibleUser)
                .WithMany()
                .HasForeignKey(e => e.ResponsibleUserId);
        }
    }
}