using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Cen.Common.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cen.Common.Data.EntityFramework
{
    public abstract class DbContextSeeder
    {
        public abstract Task EnsureSeedData(DbContext seed, string seedDataPath);

        protected static async Task Seed<TEntity>(
            DbContext context, DbSet<TEntity> dbSet, 
            string fileName, JsonSerializerOptions jsonSerializerOptions
            ) where TEntity : DataModel
        {
            var data = await ReadSeedDataFromJsonFile<TEntity[]>(Path.Combine(fileName), jsonSerializerOptions);
            await dbSet.AddRangeAsync(data);
        }

        private static async Task<TEntity> ReadSeedDataFromJsonFile<TEntity>(string fileName, JsonSerializerOptions jsonSerializerOptions)
        {
            await using var fileStream = File.OpenRead(fileName);
            var result = await JsonSerializer.DeserializeAsync<TEntity>(fileStream, jsonSerializerOptions);

            return result;
        }
    }
}