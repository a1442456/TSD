using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PurchaseTaskLineStateRowConfiguration: IEntityTypeConfiguration<PurchaseTaskLineStateRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseTaskLineStateRow> builder)
        {
            builder
                .HasOne(purchaseTaskLineStateRow => purchaseTaskLineStateRow.PurchaseTaskLine)
                .WithOne(purchaseTaskLineRow => purchaseTaskLineRow.PurchaseTaskLineState)
                .HasForeignKey<PurchaseTaskLineStateRow>(e => e.PurchaseTaskLineId);
        }
    }
}