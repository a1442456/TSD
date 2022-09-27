using System.IO;
using System.Windows.Forms;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Auth;
using Cen.Wms.Client.Models.State;
using Cen.Wms.Client.Services;
using Cen.Wms.Client.Utils;
using Newtonsoft.Json;

namespace Cen.Wms.Client
{
    public class App : IRunnable
    {
        public void Run()
        {
            TimeUtils.SetCustomTimeZone();
            EdDsaHelpers.InitSecurityKeys(Consts.PrivateKeyFileName, Consts.PublicKeyFileName);
            InitSettingsApp();

            using (var formWelcome = new WelcomeForm())
                Application.Run(formWelcome);
        }

        public void InitSettingsApp()
        {
            var settingsFilePath = SettingsApp.GetSettingsFilePath();
            if (File.Exists(settingsFilePath))
            {
                using (var file = File.OpenText(settingsFilePath))
                {
                    var serializer = new JsonSerializer();
                    var settingsApp = (SettingsApp) serializer.Deserialize(file, typeof (SettingsApp));

                    GStateProvider.Instance.SetSettingsApp(settingsApp);
                }
            }
            else
            {
                GStateProvider.Instance.SetSettingsApp(SettingsApp.GetDefaults());
            }
        }
    }
}
