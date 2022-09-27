using System;
using AutoMapper;

namespace Cen.Common.Mapping
{
    public class NullableGuidToGuidConverter: ITypeConverter<Guid?, Guid>
    {
        public Guid Convert(Guid? source, Guid destination, ResolutionContext context)
        {
            if (!source.HasValue)
                throw new ArgumentException();

            return source.Value;
        }
    }
}