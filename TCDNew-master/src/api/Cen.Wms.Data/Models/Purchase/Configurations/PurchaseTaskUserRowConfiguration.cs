using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PurchaseTaskUserRowConfiguration: IEntityTypeConfiguration<PurchaseTaskUserRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseTaskUserRow> builder)
        {
            builder
                .HasOne(purchaseTaskUserRow => purchaseTaskUserRow.PurchaseTaskHead)
                .WithMany(purchaseTaskHeadRow => purchaseTaskHeadRow.Users)
                .HasForeignKey(e => e.PurchaseTaskHeadId);
            
            builder
                .HasOne(purchaseTaskUserRow => purchaseTaskUserRow.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        }
    }
}