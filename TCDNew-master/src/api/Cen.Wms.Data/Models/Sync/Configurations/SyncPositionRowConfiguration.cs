using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Sync.Configurations
{
    public class SyncPositionRowConfiguration: IEntityTypeConfiguration<SyncPositionRow>
    {
        public void Configure(EntityTypeBuilder<SyncPositionRow> builder)
        {
            builder.HasIndex(syncPositionRow => syncPositionRow.Name);
        }
    }
}