using System;

namespace Cen.Common.CQRS.Abstract
{
    public interface IUserIdProvider
    {
        string UserId { get; }
        Guid UserGuid { get; }
    }
}