using Cen.Common.Text;
using Microsoft.EntityFrameworkCore;

namespace Cen.Common.Data.EntityFramework.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void UseSnakeCase(this ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName().Replace("Row", "").ToSnakeCase());

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().Replace("Row", "").ToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().Replace("Row", "").ToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().Replace("Row", "").ToSnakeCase());
                }
            }
        }

        public static void UseXminAsConcurrencyToken(this ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                builder.Entity(entityType.ClrType).UseXminAsConcurrencyToken();
            }
        }
    }
}