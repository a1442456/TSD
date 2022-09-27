using Cen.Wms.Client.Models.State;

namespace Cen.Wms.Client.Services
{
    class GStateProvider
    {
        private static GStateProvider _instance;

        public static GStateProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GStateProvider();

                return _instance;
            }
        }

        private SettingsApp _settingsApp;
        private SettingsUser _settingsUser;
        private SettingsFacility _settingsFacility;
        private StateAuth _stateAuth;

        public SettingsApp SettingsApp { get { return _settingsApp; } }
        public SettingsUser SettingsUser { get { return _settingsUser; } }
        public SettingsFacility SettingsFacility { get { return _settingsFacility; } }
        public StateAuth StateAuth { get { return _stateAuth; } }

        public void SetSettingsUser(SettingsUser settingsUser)
        {
            this._settingsUser = settingsUser;
        }

        public void SetSettingsApp(SettingsApp settingsApp)
        {
            this._settingsApp = settingsApp;
        }

        public void SetSettingsFacility(SettingsFacility settingsFacility)
        {
            this._settingsFacility = settingsFacility;
        }

        public void SetStateAuth(StateAuth stateAuth)
        {
            this._stateAuth = stateAuth;
        }
    }
}
