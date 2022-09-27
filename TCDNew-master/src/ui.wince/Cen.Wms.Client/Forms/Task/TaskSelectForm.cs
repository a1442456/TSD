using System;
using System.Windows.Forms;
using Cen.Wms.Client.Models.Enums;
using Cen.Wms.Client.Services;

namespace Cen.Wms.Client.Forms.Task
{
    public partial class TaskSelectForm : Form
    {
        private TaskType _result = TaskType.None;

        public TaskSelectForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            btnPurchaseByPapers.Visible = GStateProvider.Instance.SettingsFacility.IsAcceptanceByPapersEnabled;
        }

        public TaskType Result
        {
            get { return _result; }
        }

        private void btnPurchaseByTask_Click(object sender, EventArgs e)
        {
            _result = TaskType.PurchaseByTask;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnPurchaseByPapers_Click(object sender, EventArgs e)
        {
            _result = TaskType.PurchaseByPapers;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            _result = TaskType.Exit;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}