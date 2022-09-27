using System.Threading.Tasks;

namespace Cen.Common.CQRS.Abstract
{
    public interface IQueryProcessor<in TReq, TResp>
    {
        Task<TResp> Run(IUserIdProvider userIdProvider, TReq request);
    }
}