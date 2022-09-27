using System.Data;

namespace Cen.Common.Data.Sql.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IDbCommandExtension
    {
        public static IDbDataParameter AddParameter(this IDbCommand command, string name, DbType type, ParameterDirection direction)
        {
            var parentIdParameter = command.CreateParameter();
            parentIdParameter.ParameterName = name;
            parentIdParameter.DbType = type;
            parentIdParameter.Direction = direction;
            command.Parameters.Add(parentIdParameter);
            
            return parentIdParameter;
        }
    }
}