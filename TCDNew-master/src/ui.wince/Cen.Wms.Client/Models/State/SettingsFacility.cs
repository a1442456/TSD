using Cen.Wms.Client.Models.Enums;

namespace Cen.Wms.Client.Models.State
{
    class SettingsFacility
    {
        public string FacilityId { get; set; }
        public AcceptanceProcessType AcceptanceProcessType { get; set; }
        public string PalletCodePrefix { get; set; }
        public bool IsAcceptanceByPapersEnabled { get; set; }
    }
}
