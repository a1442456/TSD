using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cen.Common.Sync.Interfaces
{
    public interface ISyncSource<I, O>
    {
        Task<long> Count(ISyncPositionsStore positionsStore, string stepEntityName, I syncParameter);
        IAsyncEnumerable<O> AsEnumerable(ISyncPositionsStore positionsStore, string stepEntityName, I syncParameter);
    }
}