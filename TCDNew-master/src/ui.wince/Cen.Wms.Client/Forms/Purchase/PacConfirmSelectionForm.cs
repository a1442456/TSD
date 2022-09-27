using System.Windows.Forms;
using Cen.Wms.Client.Models.Dtos;

namespace Cen.Wms.Client.Forms.Purchase
{
    public partial class PacConfirmSelectionForm : Form
    {
        private PacHeadDto _pacHead;

        public PacConfirmSelectionForm(PacHeadDto pacHead)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            InitForm(pacHead);
        }

        public void InitForm(PacHeadDto pacHead)
        {
            _pacHead = pacHead;

            lblIdS.Text = _pacHead.PacCode;
            lblSupplierS.Text = _pacHead.SupplierName;
            lblGateS.Text = _pacHead.Gate;
        }
    }
}