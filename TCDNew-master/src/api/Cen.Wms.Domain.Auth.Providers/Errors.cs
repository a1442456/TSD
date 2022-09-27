using Cen.Common.CQRS;

namespace Cen.Wms.Domain.Auth.Providers
{
    public static class Errors
    {
        public static readonly RpcError UserNotFound = new RpcError { ErrorCode = "A001", ErrorText = "Пользователь не найден!" };
    }
}