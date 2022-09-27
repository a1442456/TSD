using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Facility.Configurations
{
    public class FacilityAccessRowConfiguration: IEntityTypeConfiguration<FacilityAccessRow>
    {
        public void Configure(EntityTypeBuilder<FacilityAccessRow> builder)
        {
            
        }
    }
}