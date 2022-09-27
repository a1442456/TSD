using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PurchaseTaskLineUpdateRowConfiguration: IEntityTypeConfiguration<PurchaseTaskLineUpdateRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseTaskLineUpdateRow> builder)
        {
            builder
                .HasOne(purchaseTaskLineRow => purchaseTaskLineRow.PurchaseTaskHead)
                .WithMany(purchaseTaskHeadRow => purchaseTaskHeadRow.LineUpdates)
                .HasForeignKey(e => e.PurchaseTaskHeadId);
            
            // builder
            //     .HasOne(purchaseTaskLineStateRow => purchaseTaskLineStateRow.PurchaseTaskLine)
            //     .WithOne(purchaseTaskLineRow => purchaseTaskLineRow.)
            //     .HasForeignKey<PurchaseTaskLineStateRow>(e => e.PurchaseTaskLineId);
        }
    }
}