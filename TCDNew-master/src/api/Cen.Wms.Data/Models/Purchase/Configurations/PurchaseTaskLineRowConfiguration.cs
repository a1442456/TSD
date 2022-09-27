using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PurchaseTaskLineRowConfiguration: IEntityTypeConfiguration<PurchaseTaskLineRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseTaskLineRow> builder)
        {
            builder
                .HasOne(purchaseTaskLineRow => purchaseTaskLineRow.PurchaseTaskHead)
                .WithMany(purchaseTaskHeadRow => purchaseTaskHeadRow.Lines)
                .HasForeignKey(e => e.PurchaseTaskHeadId);
            
            builder
                .HasOne(purchaseTaskLineRow => purchaseTaskLineRow.PurchaseTaskLineState)
                .WithOne(purchaseTaskLineStateRow => purchaseTaskLineStateRow.PurchaseTaskLine);
            
            builder
                .HasMany(purchaseTaskHeadRow => purchaseTaskHeadRow.PurchaseTaskLinePalletedStates)
                .WithOne(purchaseTaskLinePalletedStateRow => purchaseTaskLinePalletedStateRow.PurchaseTaskLine);
        }
    }
}