using System;
using System.IO;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Models.State;
using Cen.Wms.Client.Services;
using Cen.Wms.Client.Utils;
using Newtonsoft.Json;

namespace Cen.Wms.Client.Forms.Settings
{
    public partial class SettingsEditForm : Form
    {
        public SettingsEditForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            this.tbWMSServiceBaseAddress.Text = GStateProvider.Instance.SettingsApp.WMSServiceBaseAddress;
            this.tbPublicKey.Text = EdDsaHelpers.GetPublicKey(Consts.PublicKeyFileName);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text != "1204")
            {
                ShowModalMessage.Run(Messages.ErrorAuthorisation, Messages.ErrorPasswordIncorrect);
                return;
            }

            var newSettings = new SettingsApp {WMSServiceBaseAddress = tbWMSServiceBaseAddress.Text};
            using (StreamWriter file = File.CreateText(SettingsApp.GetSettingsFilePath()))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, newSettings);
            }
            GStateProvider.Instance.SetSettingsApp(newSettings);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}