using System.Threading.Tasks;

namespace Cen.Common.Sync.Interfaces
{
    public interface ISyncPositionProvider
    {
        Task<long> GetCurrentPosition(string stepEntityName);
    }
}