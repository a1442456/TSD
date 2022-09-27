using System;
using Cen.Common.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Cen.Common.Data.EntityFramework
{
    public abstract class DesignTimeDbContextFactory<T> : IDesignTimeDbContextFactory<T>
        where T : DbContext
    {
        public T CreateDbContext(string[] args)
        {
            var fileConfigSource = new FileConfigSource();
            var configuration = fileConfigSource.GetCurrentConfiguration();
            var connectionString = configuration.GetConnectionString(DbConnectionStringName);

            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseNpgsql(
                connectionString,
                x => x.UseNodaTime().MigrationsAssembly(MigrationsAssemblyName)
            );
            
            var dbContext = (T)Activator.CreateInstance(typeof(T), optionsBuilder.Options);
            return dbContext;
        }

        protected abstract string DbConnectionStringName { get; }
        protected abstract string MigrationsAssemblyName { get; }
    }
}