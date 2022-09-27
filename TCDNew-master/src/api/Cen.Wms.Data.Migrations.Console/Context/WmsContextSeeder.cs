using System.Threading.Tasks;
using Cen.Common.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Data.Migrations.Console.Context
{
    public class WmsContextSeeder: DbContextSeeder
    {
        public override async Task EnsureSeedData(DbContext seed, string seedDataPath)
        {
            throw new System.NotImplementedException();
        }
    }
}