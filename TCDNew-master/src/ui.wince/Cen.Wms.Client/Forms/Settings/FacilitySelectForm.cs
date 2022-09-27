using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.Data.Settings;
using Cen.Wms.Client.Models.Dtos;

namespace Cen.Wms.Client.Forms.Settings
{
    public partial class FacilitySelectForm : Form
    {
        private string _result;
        private readonly BindingList<ViewModelSimple> _bindingListItems;

        public FacilitySelectForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _bindingListItems = new BindingList<ViewModelSimple>();
            _bindingListItems.RaiseListChangedEvents = true;

            lbFacilities.DisplayMember = "Name";
            lbFacilities.DataSource = _bindingListItems;

            btnOK.Enabled = false;

            UpdateFacilityList();
            checkPurchaseTasksTimer.Enabled = true;
        }

        public string Result
        {
            get { return _result; }
        }

        private void OnTimer(object sender, System.EventArgs e)
        {
            UpdateFacilityList();
        }

        private void UpdateFacilityList()
        {
            var facilities = FacilityListSimpleReadByPerson.Run().OrderBy(e => e.Name).ToList();
            
            if (!facilities.Any(e => e.Id == _result))
            {
                _result = null;
            }
            _bindingListItems.Clear();
            foreach (var purchaseTask in facilities)
            {
                _bindingListItems.Add(purchaseTask);
            }
        }

        private void lbPurchaseTasks_SelectedValueChanged(object sender, System.EventArgs e)
        {
            var selectedPurchaseTask = (ViewModelSimple)(lbFacilities.SelectedIndex != -1 ? lbFacilities.SelectedItem : null);
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