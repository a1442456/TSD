using System;
using System.Collections;
using System.Collections.Generic;
using NodaTime;

namespace Cen.Common.Http.Server.OpenApi
{
    class SchemaElement
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public Type ElementType { get; set; }
        public Dictionary<string, SchemaElement> DataMembers { get; set; } = new Dictionary<string, SchemaElement>();
        public List<SchemaElement> GenericTypes { get; set; } = new List<SchemaElement>();
        public bool IsGeneric()
        {
            return GenericTypes.Count > 0;
        }
        public bool HasDataMembers()
        {
            return DataMembers.Count > 0;
        }
        public bool HasDependencies()
        {
            return DataMembers.Count > 0 || GenericTypes.Count > 0;
        }
        public bool IsArray()
        {
            return (ElementType.IsArray  || 
                (ElementType.GetInterface(nameof(IEnumerable)) != null) ||
                (ElementType.GetInterface(nameof(ICollection)) != null));
        }

        public bool IsSimple()
        {
            return (ElementType.IsPrimitive ||
                ElementType == typeof(string) ||
                ElementType == typeof(decimal) ||
                ElementType == typeof(Instant) ||
                ElementType == typeof(LocalDate) ||
                ElementType == typeof(LocalTime) ||
                ElementType == typeof(Guid));
        }

        public bool IsSimpleNullable()
        {
            if (!IsNullable())
            {
                return false;
            }
            return Nullable.GetUnderlyingType(ElementType).IsPrimitive;
        }

        public bool IsNullable()
        {
            return Nullable.GetUnderlyingType(ElementType) != null;
        }
    }
}
