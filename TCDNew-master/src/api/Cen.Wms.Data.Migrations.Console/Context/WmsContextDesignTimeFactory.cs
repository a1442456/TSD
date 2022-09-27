using Cen.Common.Data.EntityFramework;
using Cen.Wms.Common;

namespace Cen.Wms.Data.Migrations.Console.Context
{
    public class WmsContextDesignTimeFactory: DesignTimeDbContextFactory<Cen.Wms.Data.Context.WmsContext>
    {
        protected override string DbConnectionStringName => ConstsApp.DbConnectionStringName;
        protected override string MigrationsAssemblyName => ConstsApp.MigrationsAssemblyName;
    }
}