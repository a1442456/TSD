using Cen.Wms.Data.Models.Purchase.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Purchase.Configurations
{
    public class PurchaseTaskHeadRowConfiguration: IEntityTypeConfiguration<PurchaseTaskHeadRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseTaskHeadRow> builder)
        {
            builder.HasIndex(purchaseTaskHeadRow => purchaseTaskHeadRow.Code);
            
            builder
                .Property(purchaseTaskHeadRow => purchaseTaskHeadRow.PurchaseTaskState).HasDefaultValueSql(((int)PurchaseTaskState.Created).ToString());
            
            builder
                .HasMany(purchaseTaskHeadRow => purchaseTaskHeadRow.Lines)
                .WithOne(purchaseTaskLineRow => purchaseTaskLineRow.PurchaseTaskHead);
            
            builder
                .HasMany(purchaseTaskHeadRow => purchaseTaskHeadRow.LineUpdates)
                .WithOne(purchaseTaskLineUpdateRow => purchaseTaskLineUpdateRow.PurchaseTaskHead);
            
            builder
                .HasMany(purchaseTaskHeadRow => purchaseTaskHeadRow.Users)
                .WithOne(purchaseTaskUserRow => purchaseTaskUserRow.PurchaseTaskHead);

            builder
                .HasOne(purchaseTaskHeadRow => purchaseTaskHeadRow.Facility)
                .WithMany()
                .HasForeignKey(purchaseTaskHeadRow => purchaseTaskHeadRow.FacilityId);
            
            builder
                .HasOne(purchaseTaskHeadRow => purchaseTaskHeadRow.CreatedByUser)
                .WithMany()
                .HasForeignKey(purchaseTaskHeadRow => purchaseTaskHeadRow.CreatedByUserId);

            builder.HasIndex(e => e.IsPubliclyAvailable);
        }
    }
}