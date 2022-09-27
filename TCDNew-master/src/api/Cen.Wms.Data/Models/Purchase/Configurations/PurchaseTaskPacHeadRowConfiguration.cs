using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PurchaseTaskPacHeadRowConfiguration: IEntityTypeConfiguration<PurchaseTaskPacHeadRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseTaskPacHeadRow> builder)
        {
            builder
                .HasOne(purchaseTaskLineRow => purchaseTaskLineRow.PacHead)
                .WithMany()
                .HasForeignKey(e => e.PacHeadId);
            
            builder
                .HasOne(purchaseTaskLineRow => purchaseTaskLineRow.PurchaseTaskHead)
                .WithMany(purchaseTaskHeadRow => purchaseTaskHeadRow.PacHeads)
                .HasForeignKey(e => e.PurchaseTaskHeadId);
        }
    }
}