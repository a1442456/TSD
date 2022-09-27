using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Facility.Configurations
{
    public class FacilityRowConfiguration: IEntityTypeConfiguration<FacilityRow>
    {
        public void Configure(EntityTypeBuilder<FacilityRow> builder)
        {
            builder.HasIndex(facilityRow => facilityRow.ExtId);
            builder.HasIndex(facilityRow => facilityRow.ChangedAt);
        }
    }
}