using Cen.Wms.Client.Services;

namespace Cen.Wms.Client.Common
{
    public class Urls
    {
        private const string TimeRead =                             "time/read";
        private const string UserLogin =                            "auth/login";
        private const string FacilityListSimpleReadByPerson =       "facility/list/simple/by_person";
        private const string FacilityConfigGet =                    "facility/config/get";
        private const string PacHeadReadByBarcode =                 "pac/head/read";
        private const string PurchaseTaskListByPerson =             "purchase/task/list/by_person";
        private const string PurchaseTaskCreateFromPacs =           "purchase/task/create/from_pacs";
        private const string PurchaseTaskUpdateRead =               "purchase/task/update/read";
        private const string PurchaseTaskLineReadByBarcode =        "purchase/task/line/read/by_barcode";
        private const string PurchaseTaskLineUpdatePost =           "purchase/task/line/update/post";
        private const string PurchaseTaskFinish =                   "purchase/task/finish";

        public static string TimeReadUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + TimeRead; } }
        public static string UserLoginUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + UserLogin; } }
        public static string FacilityListSimpleReadByPersonUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + FacilityListSimpleReadByPerson; } }
        public static string FacilityConfigGetUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + FacilityConfigGet; } }
        public static string PacHeadReadByBarcodeUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + PacHeadReadByBarcode; } }
        public static string PurchaseTaskListByPersonUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + PurchaseTaskListByPerson; } }
        public static string PurchaseTaskCreateFromPacsUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + PurchaseTaskCreateFromPacs; } }
        public static string PurchaseTaskUpdateReadUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + PurchaseTaskUpdateRead; } }
        public static string PurchaseTaskLineReadByBarcodeUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + PurchaseTaskLineReadByBarcode; } }
        public static string PurchaseTaskLineUpdatePostUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + PurchaseTaskLineUpdatePost; } }
        public static string PurchaseTaskFinishUrl { get { return GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress + PurchaseTaskFinish; } }
    }
}
