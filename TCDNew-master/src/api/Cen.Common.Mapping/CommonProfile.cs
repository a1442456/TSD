using System;
using AutoMapper;

namespace Cen.Common.Mapping
{
    public class CommonProfile: Profile
    {
        public CommonProfile()
        {
            CreateMap<Guid, Guid?>().ConvertUsing<GuidToNullableGuidConverter>();
            CreateMap<Guid?, Guid>().ConvertUsing<NullableGuidToGuidConverter>();
        }
    }
}