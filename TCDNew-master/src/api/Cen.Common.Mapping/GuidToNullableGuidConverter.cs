using System;
using AutoMapper;

namespace Cen.Common.Mapping
{
    public class GuidToNullableGuidConverter: ITypeConverter<Guid, Guid?>
    {
        public Guid? Convert(Guid source, Guid? destination, ResolutionContext context)
        {
            if (source == Guid.Empty)
                return null;

            return source;
        }
    }
}