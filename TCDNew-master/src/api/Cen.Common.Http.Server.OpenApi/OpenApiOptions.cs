using System;
using System.Collections.Generic;

namespace Cen.Common.Http.Server.OpenApi
{
    /// <summary>
    /// A class that allows you to configure OpenApi inside of Carter
    /// </summary>
    public class OpenApiOptions
    {
        /// <summary>
        /// The title of the OpenApi document
        /// </summary>
        public string DocumentTitle { get; set; } = "Carter <3 OpenApi";

        /// <summary>
        /// The version of the API defined by the OpenApi document
        /// </summary>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// The servers property of the OpenApi document
        /// </summary>
        public IEnumerable<string> ServerUrls { get; set; } = Array.Empty<string>();

        /// <summary>
        /// The available security definitions of the OpenApi document
        /// </summary>
        public Dictionary<string, OpenApiSecurity> Securities { get; set; } = new Dictionary<string, OpenApiSecurity>();

        /// <summary>
        /// The global security definitions that apply to the api that OpenApi describes
        /// </summary>
        public IEnumerable<string> GlobalSecurityDefinitions { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Determines if OpenAPI should be mapped and enabled. True by default.
        /// </summary>
        public bool Enabled { get; set; } = true;
        
        /// <summary>
        /// Attribute used to ignore properties from the OpenAPI schema
        ///
        /// Note: Set this to null to disable.
        /// </summary>
        public Type SchemaIgnoreAttribute { get; set; } = typeof(System.Text.Json.Serialization.JsonIgnoreAttribute);
    }
}