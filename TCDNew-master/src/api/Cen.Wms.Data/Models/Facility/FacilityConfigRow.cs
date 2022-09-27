using Cen.Common.Domain.Models;
using Cen.Wms.Domain.Facility.Config.Enums;

namespace Cen.Wms.Data.Models.Facility
{
    public class FacilityConfigRow: DataModel
    {
        public AcceptanceProcessType AcceptanceProcessType { get; set; }
        public string PalletCodePrefix { get; set; }
        public bool IsAcceptanceByPapersEnabled { get; set; }
    }
}