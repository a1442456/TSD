using Cen.Wms.Data.Models.User;
using Cen.Wms.Data.Models.User.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Data.Context
{
    public partial class WmsContext
    {
        public DbSet<UserRow> User { get; set; }
        public DbSet<UserConfigRow> UserConfig { get; set; }
        public DbSet<UserStateRow> UserState { get; set; }
        
        public void UserApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserRowConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfigRowConfiguration());
            modelBuilder.ApplyConfiguration(new UserStateRowConfiguration());
        }
    }
}