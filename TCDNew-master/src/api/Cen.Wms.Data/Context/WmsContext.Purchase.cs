using Cen.Wms.Data.Models.Purchase;
using Cen.Wms.Data.Models.Purchase.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Data.Context
{
    public partial class WmsContext
    {
        public DbSet<PacHeadRow> PacHead { get; set; }
        public DbSet<PacStateRow> PacState { get; set; }
        public DbSet<PacLineRow> PacLine { get; set; }
        public DbSet<PacLineStateRow> PacLineState { get; set; }
        
        public DbSet<PurchaseTaskHeadRow> PurchaseTaskHead { get; set; }
        public DbSet<PurchaseTaskLineRow> PurchaseTaskLine { get; set; }
        public DbSet<PurchaseTaskLineStateRow> PurchaseTaskLineState { get; set; }
        public DbSet<PurchaseTaskLinePalletedStateRow> PurchaseTaskLinePalletedState { get; set; }
        public DbSet<PurchaseTaskLineUpdateRow> PurchaseTaskLineUpdate { get; set; }
        public DbSet<PurchaseTaskPalletRow> PurchaseTaskPallet { get; set; }
        public DbSet<PurchaseTaskPacHeadRow> PurchaseTaskPacHead { get; set; }
        public DbSet<PurchaseTaskUserRow> PurchaseTaskUser { get; set; }
        
        public void PurchaseApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<long>(PurchaseTaskHeadRow.ClassSequenceName);
            
            modelBuilder.ApplyConfiguration(new PacHeadRowConfiguration());
            modelBuilder.ApplyConfiguration(new PacStateRowConfiguration());
            modelBuilder.ApplyConfiguration(new PacLineRowConfiguration());
            modelBuilder.ApplyConfiguration(new PacLineStateRowConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseTaskHeadRowConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseTaskLineRowConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseTaskLineStateRowConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseTaskLinePalletedStateRowConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseTaskLineUpdateRowConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseTaskPalletRowConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseTaskPacHeadRowConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseTaskUserRowConfiguration());
        }
    }
}