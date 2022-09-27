using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class TimeReadQuery: IQueryProcessor<object, RpcResponse<Instant>>
    {
        private readonly IClock _clock;

        public TimeReadQuery(IClock clock)
        {
            _clock = clock;
        }
        
        public Task<RpcResponse<Instant>> Run(IUserIdProvider userIdProvider, object request)
        {
            return Task.FromResult(RpcResponse<Instant>.WithSuccess(_clock.GetCurrentInstant()));
        }
    }
}