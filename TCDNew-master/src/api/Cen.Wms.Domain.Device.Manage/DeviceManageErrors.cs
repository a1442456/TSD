using System;
using Cen.Common.CQRS;

namespace Cen.Wms.Domain.Device.Manage
{
    public static class DeviceManageErrors
    {
        public static readonly RpcError DeviceIsAlreadyRegistered = new RpcError { ErrorCode = "DEV000", ErrorText = "Устройство уже существует" };
    }
}