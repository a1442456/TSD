using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PurchaseTaskLinePalletedStateRowConfiguration: IEntityTypeConfiguration<PurchaseTaskLinePalletedStateRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseTaskLinePalletedStateRow> builder)
        {
            builder
                .HasOne(purchaseTaskLinePalletedStateRow => purchaseTaskLinePalletedStateRow.PurchaseTaskLine)
                .WithMany(purchaseTaskLineRow => purchaseTaskLineRow.PurchaseTaskLinePalletedStates)
                .HasForeignKey(e => e.PurchaseTaskLineId);
        }
    }
}