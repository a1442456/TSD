using NodaTime;

namespace Cen.Wms.Domain.Sync.Providers.LsFusion.Dtos
{
    public class LsFusionReqPacInterval
    {
        private readonly long _pacDateTimeFrom;
        private readonly long _pacDateTimeTo;

        public LsFusionReqPacInterval(Instant pacDateTimeFrom, Instant pacDateTimeTo)
        {
            _pacDateTimeFrom = pacDateTimeFrom.ToUnixTimeSeconds();
            _pacDateTimeTo = pacDateTimeTo.ToUnixTimeSeconds();
        }

        public long PacDateTimeFrom => _pacDateTimeFrom;

        public long PacDateTimeTo => _pacDateTimeTo;
    }
}