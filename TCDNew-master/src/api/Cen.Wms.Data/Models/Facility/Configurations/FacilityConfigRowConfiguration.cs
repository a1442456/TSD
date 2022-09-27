using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cen.Wms.Data.Models.Facility.Configurations
{
    public class FacilityConfigRowConfiguration: IEntityTypeConfiguration<FacilityConfigRow>
    {
        public void Configure(EntityTypeBuilder<FacilityConfigRow> builder)
        {
            
        }
    }
}