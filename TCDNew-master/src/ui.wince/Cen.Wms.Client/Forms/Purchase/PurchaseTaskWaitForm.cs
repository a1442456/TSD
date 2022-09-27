using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.Data.Purchase;
using Cen.Wms.Client.Models.Dtos;

namespace Cen.Wms.Client.Forms.Purchase
{
    public partial class PurchaseTaskWaitForm : Form
    {
        private string _result;
        private readonly BindingList<PurchaseTaskDto> _bindingListItems;

        public PurchaseTaskWaitForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _bindingListItems = new BindingList<PurchaseTaskDto>();
            _bindingListItems.RaiseListChangedEvents = true;

            lbPurchaseTasks.DisplayMember = "DisplayText";
            lbPurchaseTasks.DataSource = _bindingListItems;

            btnOK.Enabled = false;

            UpdatePurchaseTasksList();
            checkPurchaseTasksTimer.Enabled = true;
        }

        public string Result
        {
            get { return _result; }
        }

        private void OnTimer(object sender, System.EventArgs e)
        {
            UpdatePurchaseTasksList();
        }

        private void UpdatePurchaseTasksList()
        {
            var tasks = PurchaseTaskListReadByPerson.Run().OrderByDescending(e => e.CreatedAt).ToList();
            
            if (!tasks.Any(e => e.Id == _result))
            {
                _result = null;
            }
            _bindingListItems.Clear();
            foreach (var purchaseTask in tasks)
            {
                _bindingListItems.Add(purchaseTask);
            }
        }

        private void lbPurchaseTasks_SelectedValueChanged(object sender, System.EventArgs e)
        {
            var selectedPurchaseTask = (PurchaseTaskDto)(lbPurchaseTasks.SelectedIndex != -1 ? lbPurchaseTasks.SelectedItem : null);
            _result = selectedPurchaseTask != null ? selectedPurchaseTask.Id : null;
            btnOK.Enabled = _result != null;
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            this.checkPurchaseTasksTimer.Enabled = false;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.checkPurchaseTasksTimer.Enabled = false;

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}