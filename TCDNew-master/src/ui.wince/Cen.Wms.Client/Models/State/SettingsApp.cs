using System.IO;
using Cen.Wms.Client.Common;

namespace Cen.Wms.Client.Models.State
{
    class SettingsApp
    {
        public string WMSServiceBaseAddress { get; set; }

        public static SettingsApp GetDefaults()
        {
            return new SettingsApp { WMSServiceBaseAddress = Consts.DefaultWMSServiceBaseAddress };
        }

        public static string GetSettingsFilePath()
        {
            var settingsFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            settingsFilePath += "\\";
            settingsFilePath += Consts.SettingsFileName;

            return settingsFilePath;
        }
    }
}
