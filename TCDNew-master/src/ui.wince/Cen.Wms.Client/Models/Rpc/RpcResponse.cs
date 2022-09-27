using System.Collections.Generic;
using System.Linq;

namespace Cen.Wms.Client.Models.Rpc
{
    public class RpcResponse<T>
    {
        public static RpcResponse<T> WithSuccess(T data)
        {
            return new RpcResponse<T> { Data = data };
        }

        public static RpcResponse<T> WithError(T data, RpcError error)
        {
            return new RpcResponse<T> { Data = data, Errors = new[] { error } };
        }

        public static RpcResponse<T> WithErrors(T data, IEnumerable<RpcError> errors)
        {
            return new RpcResponse<T> { Data = data, Errors = errors.ToArray() };
        }

        public RpcResponse()
        {
            
        }

        public T Data { get; set; }
        public RpcError[] Errors { get; set; }
        public bool IsSuccess
        {
            get { return ((Errors == null ? 0 : Errors.Length) == 0); }
        }
    }
}
