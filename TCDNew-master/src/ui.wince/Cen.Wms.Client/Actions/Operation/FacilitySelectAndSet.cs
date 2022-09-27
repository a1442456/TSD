using Cen.Wms.Client.Actions.Data.Settings;
using Cen.Wms.Client.Actions.UI.Settings;
using Cen.Wms.Client.Models.State;
using Cen.Wms.Client.Services;

namespace Cen.Wms.Client.Actions.Operation
{
    class FacilitySelectAndSet
    {
        public static bool Run()
        {
            var result = false;

            var selectedFacilityId = FacilitySelect.Run();
            if (selectedFacilityId != null)
            {
                var facilityConfig = FacilityConfigGet.Run(selectedFacilityId);
                if (facilityConfig != null)
                {
                    var settingsFacility = new SettingsFacility
                    {
                        FacilityId = selectedFacilityId,
                        AcceptanceProcessType = facilityConfig.AcceptanceProcessType,
                        PalletCodePrefix = facilityConfig.PalletCodePrefix,
                        IsAcceptanceByPapersEnabled = facilityConfig.IsAcceptanceByPapersEnabled
                    };

                    GStateProvider.Instance.SetSettingsFacility(settingsFacility);
                    result = true;
                }
            }

            return result;
        }
    }
}
