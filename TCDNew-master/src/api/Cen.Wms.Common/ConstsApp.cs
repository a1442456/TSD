namespace Cen.Wms.Common
{
    public static class ConstsApp
    {
        public const string MigrationsAssemblyName = "Cen.Wms.Data.Migrations";
        
        public const string DbConnectionStringName = "DbConnection";
        public const string RedisConnectionStringName = "RedisConnection";

        public const string XsrfCookieName = "XSRF-TOKEN";
        public const string XsrfHeaderName = "X-XSRF-TOKEN";
        public const string TenantIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";
    }
}