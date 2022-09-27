using System;

namespace Cen.Common.Http.Server.OpenApi
{
    public sealed class ApiSchemaAttributes : Attribute
    {
        public string Format { get; set; }
    }
}
