using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PurchaseTaskPalletRowConfiguration: IEntityTypeConfiguration<PurchaseTaskPalletRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseTaskPalletRow> builder)
        {
            builder
                .HasOne(purchaseTaskLineRow => purchaseTaskLineRow.PurchaseTaskHead)
                .WithMany(purchaseTaskHeadRow => purchaseTaskHeadRow.Pallets)
                .HasForeignKey(e => e.PurchaseTaskHeadId);
        }
    }
}