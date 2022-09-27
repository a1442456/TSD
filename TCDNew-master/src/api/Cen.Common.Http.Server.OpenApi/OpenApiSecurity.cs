namespace Cen.Common.Http.Server.OpenApi
{
    /// <summary>
    /// A class that describes the OpenApi security definitions
    /// </summary>
    public class OpenApiSecurity
    {
        public OpenApiSecurityType Type { get; set; }

        public string Name { get; set; }

        public string Scheme { get; set; }

        public string BearerFormat { get; set; }

        public OpenApiIn In { get; set; }
    }
}