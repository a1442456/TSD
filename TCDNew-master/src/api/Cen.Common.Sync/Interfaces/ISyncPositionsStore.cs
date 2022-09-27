using System.Threading.Tasks;

namespace Cen.Common.Sync.Interfaces
{
    public interface ISyncPositionsStore
    {
        Task<long> GetPosition(string stepEntityName);
        Task SetPosition(string stepEntityName, long currentPosition);
    }
}