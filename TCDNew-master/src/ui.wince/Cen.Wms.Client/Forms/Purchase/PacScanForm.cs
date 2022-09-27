using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.Data.Purchase;
using Cen.Wms.Client.Actions.UI.Purchase;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Common;
using Cen.Wms.Client.Models.Dtos;
using Cen.Wms.Client.Services;
using Datalogic.API;

namespace Cen.Wms.Client.Forms.Purchase
{
    public partial class PacScanForm : ScanBaseForm
    {
        private readonly List<PacHeadDto> _results;
        private readonly BindingList<PacHeadDto> _bindingListResults;

        public PacScanForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _results = new List<PacHeadDto>();
            _bindingListResults = new BindingList<PacHeadDto>(_results);
            _bindingListResults.RaiseListChangedEvents = true;

            lbPurchases.DisplayMember = "PacCode";
            lbPurchases.DataSource = _bindingListResults;
        }

        public List<PacHeadDto> Results
        {
            get { return _results; }
        }

        protected override void ProcessBarcode(string barcode, CodeId codeId)
        {
            var purchaseBarcode = barcode.Trim();
            if (!string.IsNullOrEmpty(purchaseBarcode))
            {
                var purchaseHead = PacHeadReadByBarcode.Run(purchaseBarcode, GStateProvider.Instance.SettingsFacility.FacilityId);
                if (purchaseHead != null)
                {
                    if (!(_bindingListResults.Any(r => r.PacId == purchaseHead.PacId)))
                    {
                        var selectionConfirmed = PacHeadConfirmSelection.Run(purchaseHead);
                        if (selectionConfirmed)
                            _bindingListResults.Add(purchaseHead);
                    }
                    else
                        ShowModalMessage.Run(Messages.TitleError, "Закупка уже добавлена в список!");
                }
            }
            else
                ShowModalMessage.Run(Messages.TitleError, "Штрих-код закупки пуст!");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}