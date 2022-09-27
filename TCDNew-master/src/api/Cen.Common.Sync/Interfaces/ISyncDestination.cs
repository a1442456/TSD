using System.Threading.Tasks;

namespace Cen.Common.Sync.Interfaces
{
    public interface ISyncDestination<I, O>
    {
        Task<long> WriteSource(
            ISyncSession session, ISyncPositionProvider positionProvider, ISyncPositionsStore positionsStore, 
            string stepEntityName, ISyncSource<I, O> syncSource, I syncParameter);
    }
}