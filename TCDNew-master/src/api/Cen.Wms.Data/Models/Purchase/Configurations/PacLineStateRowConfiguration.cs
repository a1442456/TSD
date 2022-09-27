using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PacLineStateRowConfiguration: IEntityTypeConfiguration<PacLineStateRow>
    {
        public void Configure(EntityTypeBuilder<PacLineStateRow> builder)
        {
            builder
                .HasOne(purchaseTaskLineStateRow => purchaseTaskLineStateRow.PacLine)
                .WithMany(purchaseTaskLineRow => purchaseTaskLineRow.PacLineStates)
                .HasForeignKey(e => e.PacLineId);
        }
    }
}