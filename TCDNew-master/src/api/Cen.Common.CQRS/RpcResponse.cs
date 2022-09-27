using System.Collections.Generic;
using System.Linq;

namespace Cen.Common.CQRS
{
    public class RpcResponse<T>
    {
        public static RpcResponse<T> WithSuccess(T data)
        {
            return new RpcResponse<T> { Data = data };
        }
        
        public static RpcResponse<T> WithError(T data, RpcError error)
        {
            return new RpcResponse<T> { Data = data, Errors = new [] { error } };
        }
        
        public static RpcResponse<T> WithErrors(T data, IEnumerable<RpcError> errors)
        {
            return new RpcResponse<T> { Data = data, Errors =  errors.ToArray() };
        }

        public bool IsSuccess => (Errors?.Length ?? 0) == 0;
        
        public RpcResponse()
        {
            
        }

        public T Data { get; set; }
        public RpcError[] Errors { get; set; }
    }
}