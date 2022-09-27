using System;
using System.Windows.Forms;

namespace Cen.Wms.Client.Forms.Purchase
{
    public partial class BarcodeInputForm : Form
    {
        private string _result = null;

        public BarcodeInputForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        public string Result { get { return _result; } }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            this._result = tbBarcode.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}