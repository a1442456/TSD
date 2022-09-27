using System;
using Cen.Common.CQRS;

namespace Cen.Common.Errors
{
    public static class CommonErrors
    {
        public static readonly Func<Exception, RpcError> Exception = exception => new RpcError { ErrorCode = "COMEXC", ErrorText = $"Исключительная ситуация: {exception}" };
        public static readonly Func<string, RpcError> NotFound = entity => new RpcError { ErrorCode = "COM000", ErrorText = $"Не удалось найти: {entity}" };
        public static readonly RpcError InvalidOperation = new RpcError { ErrorCode = "COM001", ErrorText = $"Операция недоступна (неверное состояние)" };
        public static readonly RpcError InvalidArgument = new RpcError { ErrorCode = "COM002", ErrorText = $"Некорректный аргумент" };
        public static readonly Func<string, RpcError> ExternalSystemError = errorText => new RpcError { ErrorCode = "COM003", ErrorText =
            $"Внешняя система: {(errorText ?? "Неизвестная ошибка")}"
        };
        public static readonly RpcError AccessDenied = new RpcError { ErrorCode = "COM004", ErrorText = $"Доступ запрещен" };
    }
}