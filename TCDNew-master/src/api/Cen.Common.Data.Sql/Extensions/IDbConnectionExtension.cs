using System.Data;

namespace Cen.Common.Data.Sql.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IDbConnectionExtension
    {
        public static void SequenceCreate(this IDbConnection dbConnection, string sequenceName)
        {
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = $"create sequence if not exists {sequenceName} start with 1 increment by 1;";
                command.ExecuteNonQuery();
            }
        }
        
        public static void SequenceSetValue(this IDbConnection dbConnection, string sequenceName, long value)
        {
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = "select setval(:sequenceName, :sequenceValue)";
                var pSequenceName = command.AddParameter(":sequenceName", DbType.String, ParameterDirection.Input);
                var pSequenceValue = command.AddParameter(":sequenceValue", DbType.Int64, ParameterDirection.Input);
                pSequenceName.Value = sequenceName;
                pSequenceValue.Value = value;
                command.ExecuteNonQuery();
            }
        }
        
        public static long? SequenceNextval(this IDbConnection dbConnection, string sequenceName)
        {
            if (string.IsNullOrEmpty(sequenceName))
                return null;

            long? result;

            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = "select nextval(:sequenceName)";
                var parameter = command.AddParameter(":sequenceName", DbType.String, ParameterDirection.Input);
                parameter.Value = sequenceName;
                result = (long?)command.ExecuteScalar();
            }

            return result;
        }
    }
}