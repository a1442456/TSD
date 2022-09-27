using System;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.Data.Purchase;
using Cen.Wms.Client.Actions.UI.Purchase;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Controls.Grid;
using Cen.Wms.Client.Models.Enums;
using Cen.Wms.Client.Services;
using Cen.Wms.Client.Models.Dtos.GosZNKAK;
using Datalogic.API;
using NLog;
using System.Drawing;
using Cen.Wms.Client.Forms.Utility;

namespace Cen.Wms.Client.Forms.Purchase
{
    public partial class PurchaseTaskContentScanForm : Common.ScanBaseForm
    {
        private BindingSource _bindingPurchaseTaskLines;
        private int _scale;
        private VGrid _vGrid;
        private bool _isInPosition;
        private PurchaseTaskStateView _taskStateView;
        GosZNAK _znakService;
        private string _purchasesTaskId = string.Empty;
        private string _gosZnakToken = string.Empty;

        public PurchaseTaskContentScanForm(string purchasesTaskId)
        {
            _znakService = new GosZNAK();
            _gosZnakToken = _znakService.GetToken();
            _purchasesTaskId = purchasesTaskId;
            InitializeComponent();
            btnInputBarcodePallet.Enabled = IsAcceptanceProcessTypeIsPalletized();
            lblPaletteStats.Visible = IsAcceptanceProcessTypeIsPalletized();
            WindowState = FormWindowState.Maximized;
            _taskStateView = new PurchaseTaskStateView(purchasesTaskId);
            InitPage();
        }

        private void InitPage()
        {
            var initialized = FillSessionLines();
            if (!initialized)
            {

                this.DialogResult = DialogResult.Abort;
                this.Close();
                return;
            }
            initialized &= InitVGrid();
            if (!initialized)
            {

                this.DialogResult = DialogResult.Abort;
                this.Close();
                return;
            }
            initialized &= InitCurrentPalletCode();
            if (!initialized)
            {

                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private bool FillSessionLines()
        {
            var result = false;
            Exception fillSessionLinesException = null;

            try
            {
                var purchaseTaskUpdate = PurchaseTaskUpdateRead.Run(_taskStateView.PurchaseTaskId, 0);
                _taskStateView.PurchaseTaskUpdateApply(purchaseTaskUpdate);

                _bindingPurchaseTaskLines = _taskStateView.GetLinesDataSource();

                result = true;
            }
            catch (Exception exception)
            {
                fillSessionLinesException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }

            if (fillSessionLinesException != null)
                ShowModalMessage.Run(Messages.TitleError, "Проблема при загрузке информации о закупке!");

            return result;
        }

        private bool InitVGrid()
        {
            var result = false;
            Exception initVGridException = null;

            try
            {
                _scale = (int)(CurrentAutoScaleDimensions.Height / 96);
                _vGrid = new VGrid(pnlGrid, _bindingPurchaseTaskLines, new RowDrawerPurchaseTaskLine(), _scale);
                _vGrid.BringToFront();

                result = true;
            }
            catch (Exception exception)
            {
                initVGridException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }

            if (initVGridException != null)
                ShowModalMessage.Run(Messages.TitleError, "Проблема при создании таблицы для отображения информации!");

            return result;
        }

        private bool InitCurrentPalletCode()
        {
            var isPalletCodeInitialised = !IsAcceptanceProcessTypeIsPalletized();
            if (!isPalletCodeInitialised)
            {
                isPalletCodeInitialised = InputBarcodePallet();
            }

            return isPalletCodeInitialised;
        }

        private bool IsAcceptanceProcessTypeIsPalletized()
        {
            return GStateProvider.Instance.SettingsFacility.AcceptanceProcessType == AcceptanceProcessType.Palletized;
        }

        private bool InputBarcodePallet()
        {
            var result = false;
            try
            {
                CleanRequests();
                var palletBarcode = BarcodePalletScan.Run();
                if (!string.IsNullOrEmpty(palletBarcode))
                {
                    _taskStateView.SetCurrentPalletCode(palletBarcode);
                    result = true;
                    UpdateStatsPanel();
                }
                else
                    ShowModalMessage.Run(Messages.TitleError, "Штрих-код палетты пуст!");
            }
            finally
            {
                RefreshRequests();
            }
            return result;
        }

        protected override void ProcessBarcode(string barcode, CodeId codeId)
        {   
            if (barcode.Length > 15) //13+CR+LF
            {
                //Удалить суффикс CR+LF
                string barcodeTrimmed = string.Empty;
                for (int i = 0; i < barcode.Length - 2; i++)
                    barcodeTrimmed += barcode[i];

                GosZNAKLabels lblInfo = new GosZNAKLabels();
                _znakService.GetLabels(barcodeTrimmed, _gosZnakToken);
            }
            else
            {
                try
                {
                    if (_isInPosition)
                        return;

                    _isInPosition = true;

                    var barcodeCleaned = barcode.Trim();
                    var purchaseTaskUpdate = PurchaseTaskUpdateRead.Run(_taskStateView.PurchaseTaskId, _taskStateView.PurchaseTaskVersion);
                    _taskStateView.PurchaseTaskUpdateApply(purchaseTaskUpdate);

                    var currentPalletCode = _taskStateView.GetCurrentPalletCode();
                    var currentPalletAbc = _taskStateView.GetCurrentPalletABC();
                    var purchaseTaskLine = PurchaseTaskLineReadByBarcode.Run(_taskStateView.PurchaseTaskId, barcodeCleaned, currentPalletCode);
                    if (purchaseTaskLine == null)
                    {
                        ShowModalMessage.Run(Messages.TitleError, "Неверный штрих-код товара!");
                        return;
                    }
                    if (IsAcceptanceProcessTypeIsPalletized())
                    {
                        if (!_taskStateView.CanPalletAcceptProductABC(currentPalletAbc, purchaseTaskLine.Product.Abc))
                        {
                            ShowModalMessage.Run(
                                Messages.TitleError,
                                string.Format(
                                    "Нельзя положить на паллету с товаром категории {0} товар категории {1}!",
                                    currentPalletAbc.ToUpper(),
                                    purchaseTaskLine.Product.Abc.ToUpper()
                                )
                            );
                            return;
                        }
                        if (_taskStateView.IsPalletABCEmpty(currentPalletAbc) && _taskStateView.IsPalletABCUsed(purchaseTaskLine.Product.Abc))
                        {
                            var useNewPalletConfirmed = ShowModalDialog.Run(
                                "Категория товара паллеты",
                                string.Format("Паллета под товары категории {0} уже используется. Уверены, что хотите использовать еще одну паллету?", purchaseTaskLine.Product.Abc.ToUpper())
                            ) == DialogResult.OK;
                            if (!useNewPalletConfirmed)
                            {
                                return;
                            }
                        }
                    }

                    var purchaseTaskLineUpdate = PurchaseTaskLineEdit.Run(purchaseTaskLine);
                    var wasUpdateSuccessful =
                        PurchaseTaskLineUpdatePost.Run(_taskStateView.PurchaseTaskId, barcodeCleaned, currentPalletCode, purchaseTaskLineUpdate);
                    if (!wasUpdateSuccessful)
                    {
                        ShowModalMessage.Run(Messages.TitleError, "Данные не сохранены на сервере! Попробуйте еще раз!");
                    }

                    purchaseTaskUpdate = PurchaseTaskUpdateRead.Run(_taskStateView.PurchaseTaskId, _taskStateView.PurchaseTaskVersion);
                    _taskStateView.PurchaseTaskUpdateApply(purchaseTaskUpdate);

                    UpdateStatsPanel();

                    RefreshRequests();
                }
                catch (Exception exception)
                {
                    ShowModalMessage.Run(Messages.TitleError, exception.ToString());
                    var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                    logger.Error(exception);
                }
                finally
                {
                    _isInPosition = false;
                }
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            Exception formPurchasePauseException = null;
            
            try
            {
                var pauseResult = PurchaseTaskPause.Run();

                switch (pauseResult)
                {
                    case DialogResult.Retry:
                        // продолжаем
                        break;

                    case DialogResult.Ignore:
                        // выходим
                        this.DialogResult = DialogResult.Ignore;
                        this.Close();
                        break;

                    case DialogResult.Cancel:
                        var confirmedCancel = PurchaseTaskConfirmDiffCount.Run(_taskStateView.GetDifferencesCount());
                        if (confirmedCancel)
                        {
                            if (PurchaseTaskFinish.Run(_taskStateView.PurchaseTaskId, true, false))
                            {
                                // закончили приемку, все ок
                                ShowModalMessage.Run(Messages.TitleMessage, "Задание успешно отменено!");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                                ShowModalMessage.Run(Messages.TitleError, "Не удалось отменено задание! Обязательно повторите попытку еще раз!!!");
                        }
                        break;

                    case DialogResult.Yes:
                        var confirmedYes = PurchaseTaskConfirmDiffCount.Run(_taskStateView.GetDifferencesCount());
                        if (confirmedYes)
                        {
                            if (PurchaseTaskFinish.Run(_taskStateView.PurchaseTaskId, false, true))
                            {
                                // закончили приемку, все ок
                                ShowModalMessage.Run(Messages.TitleMessage, "Задание успешно завершено и отправлено!");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                                ShowModalMessage.Run(Messages.TitleError, "Не удалось завершить и отправить задание! Обязательно повторите попытку еще раз!!!");
                        }
                        break;

                    case DialogResult.OK:
                        var confirmedOK = PurchaseTaskConfirmDiffCount.Run(_taskStateView.GetDifferencesCount());
                        if (confirmedOK)
                        {
                            if (PurchaseTaskFinish.Run(_taskStateView.PurchaseTaskId, false, false))
                            {
                                // закончили приемку, все ок
                                ShowModalMessage.Run(Messages.TitleMessage, "Задание успешно завершено!");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                                ShowModalMessage.Run(Messages.TitleError, "Не удалось завершить задание! Обязательно повторите попытку еще раз!!!");
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                formPurchasePauseException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }
            if (formPurchasePauseException != null)
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка выполнении операции!");

            UpdateStatsPanel();
        }

        private void btnInputBarcodeGood_Click(object sender, EventArgs e)
        {
            try
            {
                CleanRequests();
                var productBarcode = BarcodeInput.Run();
                if (!string.IsNullOrEmpty(productBarcode))
                    ProcessBarcode(productBarcode, CodeId.NoData);
                else
                    ShowModalMessage.Run(Messages.TitleError, "Штрих-код товара пуст!");
            }
            finally
            {
                RefreshRequests();
            }
        }

        private void btnInputBarcodePallet_Click(object sender, EventArgs e)
        {
            InputBarcodePallet();
        }

        private void UpdateStatsPanel()
        {
            lblPaletteStats.Text = String.Format("{0}:{1}", _taskStateView.GetCurrentPalletABC().ToUpper(), _taskStateView.GetCurrentPalletCode());
            lblStats.Text = String.Format("{0}/{1}/{2}", _taskStateView.GetAcceptedCount(), _taskStateView.GetDifferencesCount(), _taskStateView.GetTotalCount());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var purchaseTaskUpdate = PurchaseTaskUpdateRead.Run(_taskStateView.PurchaseTaskId, _taskStateView.PurchaseTaskVersion);
            _taskStateView.PurchaseTaskUpdateApply(purchaseTaskUpdate);

            UpdateStatsPanel();

            RefreshRequests();
        }

        private void btnCheckGood_Click(object sender, EventArgs e)
        {
            CheckGood frm = new CheckGood(_purchasesTaskId);
            this.Close();
            frm.Show();
        }

        private void lblStats_ParentChanged(object sender, EventArgs e)
        {

        }
    }
}
