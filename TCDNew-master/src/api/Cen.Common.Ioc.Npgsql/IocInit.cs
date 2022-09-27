using System.Data;
using SimpleInjector;
using Npgsql;

namespace Cen.Common.Ioc.Npgsql
{
    public static class IocInit
    {
        public static void UseNpgsql(this Container container, string connectionString)
        {
            container.Register(() => CreateConnectionOpen(connectionString), Lifestyle.Scoped);
        }

        private static IDbConnection CreateConnectionOpen(string connectionString)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            
            return connection;
        }
    }
}