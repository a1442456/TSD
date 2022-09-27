using System;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.Operation;
using Cen.Wms.Client.Actions.UI.Settings;
using Cen.Wms.Client.Actions.UI.Utility;

namespace Cen.Wms.Client.Forms.Auth
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
            label1.Text += Wms.Client.Settings.isTest ? "TEST" : Wms.Client.Settings.Current.ToString();
            WindowState = FormWindowState.Maximized;
        }

        private void buttonAuth_Click(object sender, EventArgs e)
        {
            if (TimeSync.Run())
            {
                var wasLoginSuccessful = ApplicationLogin.Run();
                if (wasLoginSuccessful)
                {
                    var wasFacilitySelected = FacilitySelectAndSet.Run();
                    if (wasFacilitySelected)
                    {
                        ExecuteTask.Run();
                    }
                }
            }

            this.Close();
        }

        private void btnSettingsEdit_Click(object sender, EventArgs e)
        {
            SettingsEdit.Run();
        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}