using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Common;
using Cen.Wms.Client.Services;
using Datalogic.API;

namespace Cen.Wms.Client.Forms.Purchase
{
    public partial class BarcodePalletScanForm : ScanBaseForm
    {
        private string _result;

        public BarcodePalletScanForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        public string Result
        {
            get { return _result; }
        }

        protected override void ProcessBarcode(string barcode, CodeId codeId)
        {
            var palletBarcode = barcode.Trim();
            if (!string.IsNullOrEmpty(palletBarcode))
            {
                if (palletBarcode.StartsWith(GStateProvider.Instance.SettingsFacility.PalletCodePrefix))
                {
                    _result = palletBarcode;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                    ShowModalMessage.Run(Messages.TitleError, "Сосканированный штрих-код не паллеты!");
            }
            else
                ShowModalMessage.Run(Messages.TitleError, "Штрих-код закупки пуст!");
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}