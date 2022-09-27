using System;

namespace Cen.Common.Domain.Interfaces
{
    public interface IDataWithKey
    {
        Guid? Id { get; set; }
    }
}