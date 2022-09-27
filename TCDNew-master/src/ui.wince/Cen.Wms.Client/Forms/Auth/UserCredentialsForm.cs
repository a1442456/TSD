using System;
using System.Windows.Forms;
using Cen.Wms.Client.Models.Dtos;

namespace Cen.Wms.Client.Forms.Auth
{
    public partial class UserCredentialsForm : Form
    {
        public UserCredentialsForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        public UserCredentials Result
        {   
            get
            {
                if (!Wms.Client.Settings.isTest)
                    return new UserCredentials { UserName = this.tbUser.Text, UserPassword = this.tbPassword.Text };    
                else
                    return new UserCredentials { UserName = "it3m", UserPassword = "2115147" };
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}