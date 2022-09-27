using Cen.Common.Data.DataSource.Infrastructure;

namespace Cen.Common.Data.DataSource.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class JsonObjectExtensions
    {
        public static IEnumerable<IDictionary<string, object>> ToJson(this IEnumerable<JsonObject> items)
        {          
            return items.Select(i => i.ToJson());
        }
    }
}