using System;

namespace Cen.Common.CQRS
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