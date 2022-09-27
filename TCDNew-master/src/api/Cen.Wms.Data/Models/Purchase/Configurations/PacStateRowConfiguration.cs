using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PacStateRowConfiguration: IEntityTypeConfiguration<PacStateRow>
    {
        public void Configure(EntityTypeBuilder<PacStateRow> builder)
        {
            builder.HasIndex(pacStateRow => pacStateRow.IsBusy);
            builder.HasIndex(pacStateRow => pacStateRow.IsExported);
            builder.HasIndex(pacStateRow => pacStateRow.IsProcessed);
            
            builder
                .HasOne(pacStateRow => pacStateRow.PacHead)
                .WithOne(pacHeadRow => pacHeadRow.PacState)
                .HasForeignKey<PacStateRow>(e => e.PacHeadId);
        }
    }
}