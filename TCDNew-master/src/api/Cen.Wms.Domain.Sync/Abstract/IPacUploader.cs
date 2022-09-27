using System.Collections.Generic;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Wms.Data.Models.Purchase;

namespace Cen.Wms.Domain.Sync.Abstract
{
    public interface IPacUploader
    {
        Task<RpcResponse<object>> Upload(IEnumerable<PacHeadRow> pacHeadRows, string purchaseTaskCode);
    }
}