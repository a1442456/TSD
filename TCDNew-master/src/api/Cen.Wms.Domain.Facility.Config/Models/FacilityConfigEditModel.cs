using Cen.Wms.Domain.Facility.Config.Enums;

namespace Cen.Wms.Domain.Facility.Config.Models
{
    public class FacilityConfigEditModel
    {
        public AcceptanceProcessType AcceptanceProcessType { get; set; }
        public string PalletCodePrefix { get; set; }
        public bool IsAcceptanceByPapersEnabled { get; set; }
        // public int TZBias { get; set; }
        // public int MaxExpirationDaysPlus { get; set; }

        public static FacilityConfigEditModel GetDefault()
        {
            return
                new FacilityConfigEditModel
                {
                    AcceptanceProcessType = AcceptanceProcessType.Palletized,
                    PalletCodePrefix = "22",
                    IsAcceptanceByPapersEnabled = false
                };
        }
    }
}