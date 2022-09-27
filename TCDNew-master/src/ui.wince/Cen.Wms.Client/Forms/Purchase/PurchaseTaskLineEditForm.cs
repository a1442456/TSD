using System;
using System.Drawing;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Models.Dtos;
using Cen.Wms.Client.Models.Enums;
using NLog;

namespace Cen.Wms.Client.Forms.Purchase
{
    public partial class PurchaseTaskLineEditForm : Form
    {
        private readonly PurchaseTaskLineDto _purchaseTaskLine;
        private PurchaseTaskLineUpdateDto _result;
        private readonly DateTime _dateToday;
        private readonly DateTime _dateDefault;

        private decimal FormQuantityNormalBefore
        {
            get { return !string.IsNullOrEmpty(tbQuantityBefore.Text) ? Convert.ToDecimal(tbQuantityBefore.Text) : 0; }
            set { tbQuantityBefore.Text = Convert.ToString(value); }
        }

        private decimal FormQuantityNormalAdd
        {
            get { return !string.IsNullOrEmpty(tbQuantityAdd.Text) ? Convert.ToDecimal(tbQuantityAdd.Text) : 0; }
            set
            {
                var addPosition = tbQuantityAdd.SelectionStart;
                tbQuantityAdd.Text = (value == 0 ? string.Empty : Convert.ToString(value));
                tbQuantityAdd.SelectionStart = addPosition;
            }
        }

        private decimal FormQuantityBrokenBefore
        {
            get { return !string.IsNullOrEmpty(tbBrokenBefore.Text) ? Convert.ToDecimal(tbBrokenBefore.Text) : 0; }
            set { tbBrokenBefore.Text = Convert.ToString(value); }
        }

        private decimal FormQuantityBrokenAdd
        {
            get { return !string.IsNullOrEmpty(tbBrokenAdd.Text) ? Convert.ToDecimal(tbBrokenAdd.Text) : 0; }
            set
            {
                var addPosition = tbBrokenAdd.SelectionStart;
                tbBrokenAdd.Text = (value == 0 ? string.Empty : Convert.ToString(value));
                tbBrokenAdd.SelectionStart = addPosition;
            }
        }

        private long FormExpirationDaysPlus
        {
            get { return !string.IsNullOrEmpty(tbDaysExp.Text) ? Convert.ToInt64(tbDaysExp.Text) : 0; }
            set
            {
                var addPosition = tbDaysExp.SelectionStart;
                tbDaysExp.Text = (value == 0 ? string.Empty : Convert.ToString(value));
                tbDaysExp.SelectionStart = addPosition;
            }
        }

        public PurchaseTaskLineUpdateDto Result
        {
            get { return _result; }
            private set { _result = value; }
        }

        public PurchaseTaskLineEditForm(PurchaseTaskLineDto purchaseTaskLine)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _purchaseTaskLine = purchaseTaskLine;
            _result = new PurchaseTaskLineUpdateDto(purchaseTaskLine.Id, purchaseTaskLine.State);

            var _dateNow = DateTime.Now;
            _dateToday = new DateTime(_dateNow.Year, _dateNow.Month, _dateNow.Day, 0, 0, 0, DateTimeKind.Utc);
            _dateDefault = new DateTime(_dateNow.Year, _dateNow.Month > 1 ? _dateNow.Month - 1 : 1, 01, 0, 0, 0, DateTimeKind.Utc);

            _result.PurchaseTaskLineState.QtyNormal = 1;
            if (_result.PurchaseTaskLineState.ExpirationDate == null)
            {
                if (_purchaseTaskLine.State.QtyFull == 0)
                {
                    _result.PurchaseTaskLineState.ExpirationDate = _dateDefault;
                }
            }

            InitForm();
        }

        public void InitForm()
        {
            UISetup();
            UIUpdate();

            this.tbQuantityAdd.Focus();
            this.tbQuantityAdd.SelectAll();
        }

        public void UIBind()
        {
            this.pDateExp.ValueChanged += this.pDateExp_ValueChanged;
            this.tbDaysExp.TextChanged += this.tbDaysExp_TextChanged;
            this.tbQuantityAdd.TextChanged += this.tbQuantityAdd_TextChanged;
            this.tbBrokenAdd.TextChanged += this.tbBrokenAdd_TextChanged;
            this.cbDateExp.CheckStateChanged += this.cbDateExp_CheckStateChanged;
        }

        public void UIUnbind()
        {
            this.pDateExp.ValueChanged -= this.pDateExp_ValueChanged;
            this.tbDaysExp.TextChanged -= this.tbDaysExp_TextChanged;
            this.tbQuantityAdd.TextChanged -= this.tbQuantityAdd_TextChanged;
            this.tbBrokenAdd.TextChanged -= this.tbBrokenAdd_TextChanged;
            this.cbDateExp.CheckStateChanged -= this.cbDateExp_CheckStateChanged;
        }

        public void UpdateModelZeroExpInfo()
        {
            if (!this.cbDateExp.Checked)
            {
                _result.PurchaseTaskLineState.ExpirationDate = null;
                _result.PurchaseTaskLineState.ExpirationDaysPlus = 0;
            }
            else
            {
                _result.PurchaseTaskLineState.ExpirationDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month > 1 ? DateTime.UtcNow.Month - 1 : 1, 01, 0, 0, 0, DateTimeKind.Utc);
                _result.PurchaseTaskLineState.ExpirationDaysPlus = 0;
            }
        }

        public void UpdateModelDateTimeExp()
        {
            _result.PurchaseTaskLineState.ExpirationDate = DateTime.SpecifyKind(pDateExp.Value, DateTimeKind.Utc);
        }

        public void UpdateModelDaysExp()
        {
            Exception updateModelException = null;
            try
            {
                _result.PurchaseTaskLineState.ExpirationDaysPlus = FormExpirationDaysPlus;
            }
            catch (Exception exception)
            {
                updateModelException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }

            // TODO: добавить проверку на Settings.MaxExpirationDaysPlus
            //if (_result.PurchaseTaskLineState.ExpirationDaysPlus > Settings.MaxExpirationDaysPlus)
            //{
            //    ShowModalMessage.Run(Messages.TitleError, string.Format("Количество дней не должно быть больше {0}!", Settings.MaxExpirationDaysPlus));
            //    _result.PurchaseTaskLineState.ExpirationDaysPlus = 0;
            //}

            if (updateModelException != null)
                ShowModalMessage.Run(Messages.TitleError, "Неправильный ввод количества дней!");
        }

        public void UpdateModelQtyNormal()
        {
            Exception updateModelException = null;
            try
            {
                var quantityNormalAdd = FormQuantityNormalAdd;
                if (_purchaseTaskLine.State.QtyNormal + FormQuantityNormalAdd + _purchaseTaskLine.State.QtyBroken + FormQuantityBrokenAdd > _purchaseTaskLine.Quantity)
                {
                    ShowModalMessage.Run(Messages.TitleError, "Вы не можете принять кол-во, большее чем указано в документе!");
                    quantityNormalAdd = _purchaseTaskLine.Quantity - (_purchaseTaskLine.State.QtyNormal + _purchaseTaskLine.State.QtyBroken + FormQuantityBrokenAdd);
                }
                _result.PurchaseTaskLineState.QtyNormal = quantityNormalAdd;
            }
            catch (Exception exception)
            {
                updateModelException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }
            if (updateModelException != null)
                ShowModalMessage.Run(Messages.TitleError, "Неправильный ввод количества!");
        }

        public void UpdateModelQtyBroken()
        {
            Exception updateModelException = null;
            try
            {
                var quantityBrokenAdd = FormQuantityBrokenAdd;
                //Проверка браков отключена.
                //if (_purchaseTaskLine.State.QtyNormal + FormQuantityNormalAdd + _purchaseTaskLine.State.QtyBroken + FormQuantityBrokenAdd > _purchaseTaskLine.Quantity)
                //{
                //    ShowModalMessage.Run(Messages.TitleError, "Вы не можете принять кол-во, большее чем указано в документе!");
                //    quantityBrokenAdd = _purchaseTaskLine.Quantity - (_purchaseTaskLine.State.QtyNormal + FormQuantityNormalAdd + _purchaseTaskLine.State.QtyBroken);
                //}
                _result.PurchaseTaskLineState.QtyBroken = quantityBrokenAdd;
            }
            catch (Exception exception)
            {
                updateModelException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }
            if (updateModelException != null)
                ShowModalMessage.Run(Messages.TitleError, "Неправильный ввод количества!");
        }

        public void UISetup()
        {
            lblNameS.Text = _purchaseTaskLine.Product.Name;

            if (_purchaseTaskLine.Product.Abc == "b")
                lblName.BackColor = Color.Cyan;
            if (_purchaseTaskLine.Product.Abc == "c")
                lblName.BackColor = Color.Magenta;

            FormQuantityNormalBefore = _purchaseTaskLine.State.QtyNormal;
            FormQuantityBrokenBefore = _purchaseTaskLine.State.QtyBroken;

            lblTotalS.Text = _purchaseTaskLine.Quantity.ToString();
        }

        public void UIUpdate()
        {
            UIUnbind();

            this.cbDateExp.Checked = _result.PurchaseTaskLineState.ExpirationDate.HasValue;
            this.pDateExp.Enabled = cbDateExp.Checked;
            this.tbDaysExp.Enabled = cbDateExp.Checked;

            pDateExp.Value = _result.PurchaseTaskLineState.ExpirationDate.HasValue
                ? DateTime.SpecifyKind(_result.PurchaseTaskLineState.ExpirationDate.Value, DateTimeKind.Unspecified)
                : DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Unspecified);

            FormExpirationDaysPlus = _result.PurchaseTaskLineState.ExpirationDaysPlus;


            if (FormQuantityNormalAdd != _result.PurchaseTaskLineState.QtyNormal)
                FormQuantityNormalAdd = _result.PurchaseTaskLineState.QtyNormal;


            if (FormQuantityBrokenAdd != _result.PurchaseTaskLineState.QtyBroken)
                FormQuantityBrokenAdd = _result.PurchaseTaskLineState.QtyBroken;

            lblSumQuantity.Text = Convert.ToString(_purchaseTaskLine.State.QtyNormal + _result.PurchaseTaskLineState.QtyNormal);
            lblSumBroken.Text = Convert.ToString(_purchaseTaskLine.State.QtyBroken + _result.PurchaseTaskLineState.QtyBroken);

            lblSumS.Text = (
                _purchaseTaskLine.State.QtyNormal + _result.PurchaseTaskLineState.QtyNormal +
                _purchaseTaskLine.State.QtyBroken + _result.PurchaseTaskLineState.QtyBroken
            ).ToString();

            UIBind();
        }

        private void pDateExp_ValueChanged(object sender, EventArgs e)
        {
            UpdateModelDateTimeExp();
            UIUpdate();
        }

        private void tbDaysExp_TextChanged(object sender, EventArgs e)
        {
            UpdateModelDaysExp();
            UIUpdate();
        }

        private void cbDateExp_CheckStateChanged(object sender, EventArgs e)
        {
            UpdateModelZeroExpInfo();
            UIUpdate();
        }

        private void tbQuantityAdd_TextChanged(object sender, EventArgs e)
        {
            UpdateModelQtyNormal();
            UIUpdate();
        }

        private void tbBrokenAdd_TextChanged(object sender, EventArgs e)
        {
            UpdateModelQtyBroken();
            UIUpdate();
        }

        private bool ValidateModel() {
            var result = true;

            if (_result.PurchaseTaskLineState.ExpirationDate != null) {
              if (_result.PurchaseTaskLineState.ExpirationDate.Value.AddDays(_result.PurchaseTaskLineState.ExpirationDaysPlus).CompareTo(_dateToday) < 0) {
                ShowModalMessage.Run(
                  Messages.TitleError,
                  "Истекший срок годности!"
                );

                result = false;
              }
            }

            return result;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var isValid = ValidateModel();

            if (isValid)
            {
                _result.PurchaseTaskLineUpdateType = PurchaseTaskLineUpdateType.Update;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            _result.PurchaseTaskLineUpdateType = PurchaseTaskLineUpdateType.Reset;

            this.DialogResult = DialogResult.Retry;
            this.Close();
        }
    }
}