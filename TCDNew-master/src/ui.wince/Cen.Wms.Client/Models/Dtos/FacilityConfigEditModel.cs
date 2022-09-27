using Cen.Wms.Client.Models.Enums;

namespace Cen.Wms.Client.Models.Dtos
{
    public class FacilityConfigEditModel
    {
        public AcceptanceProcessType AcceptanceProcessType { get; set; }
        public string PalletCodePrefix { get; set; }
        public bool IsAcceptanceByPapersEnabled { get; set; }
        // public int TZBias { get; set; }
        // public int MaxExpirationDaysPlus { get; set; }
    }
}
