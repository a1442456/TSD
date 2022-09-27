using System;
using Cen.Wms.Client.Models.Rpc;

namespace Cen.Wms.Client.Models.Exceptions
{
    public class RpcException: Exception
    {
        public readonly RpcError[] Errors;

        public RpcException(RpcError[] errors)
        {
            Errors = errors;
        }
    }
}
