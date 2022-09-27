using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PacLineRowConfiguration: IEntityTypeConfiguration<PacLineRow>
    {
        public void Configure(EntityTypeBuilder<PacLineRow> builder)
        {
            builder.HasIndex(pacLineRow => pacLineRow.ExtId);
            builder.HasIndex(pacLineRow => pacLineRow.ChangedAt);
            
            builder
                .HasOne(pacLineRow => pacLineRow.PacHead)
                .WithMany(pacRow => pacRow.Lines)
                .HasForeignKey(e => e.PacHeadId);
            
            builder
                .HasMany(pacLineRow => pacLineRow.PacLineStates)
                .WithOne(pacLineStateRow => pacLineStateRow.PacLine);
        }
    }
}