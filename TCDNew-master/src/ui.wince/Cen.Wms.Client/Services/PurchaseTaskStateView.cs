using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Cen.Wms.Client.Models.Dtos;

namespace Cen.Wms.Client.Services
{
    public class PurchaseTaskStateView
    {
        private readonly BindingList<PurchaseTaskLineDto> _purchaseTaskLines;
        private readonly List<PurchaseTaskPalletDto> _purchaseTaskPallets;
        private string _currentPalletCode = string.Empty;

        public readonly string PurchaseTaskId;
        public long PurchaseTaskVersion = 0;

        public PurchaseTaskStateView(string purchaseTaskId)
        {
            this.PurchaseTaskId = purchaseTaskId;
            _purchaseTaskLines = new BindingList<PurchaseTaskLineDto>();
            _purchaseTaskPallets = new List<PurchaseTaskPalletDto>();
        }

        public int GetDifferencesCount()
        {
            return _purchaseTaskLines.Count(line => (line.Quantity != line.State.QtyFull));
        }

        public int GetTotalCount()
        {
            return _purchaseTaskLines.Count();
        }

        public int GetAcceptedCount()
        {
            return _purchaseTaskLines.Count(line => (line.Quantity == line.State.QtyFull));
        }

        public void SetCurrentPalletCode(string palletCode)
        {
            _currentPalletCode = palletCode;
        }

        public string GetCurrentPalletCode()
        {
            return _currentPalletCode;
        }

        public string GetCurrentPalletABC()
        {
            var purchaseTaskPallet = _purchaseTaskPallets.FirstOrDefault(e => e.Code == _currentPalletCode);
            return purchaseTaskPallet != null ? purchaseTaskPallet.ABC : string.Empty;
        }

        public bool IsPalletABCUsed(string abc)
        {
            var purchaseTaskPallet = _purchaseTaskPallets.FirstOrDefault(e => e.ABC.Contains(abc.Trim().ToLower()));
            return purchaseTaskPallet != null;
        }

        public bool IsPalletABCEmpty(string abc)
        {
            return abc.Trim() == string.Empty;
        }

        public bool CanPalletAcceptProductABC(string palletABC, string goodABC)
        {
            return ((palletABC.ToLower() == "") || (palletABC.ToLower().Contains(goodABC.Trim().ToLower())));
        }

        public void PurchaseTaskUpdateApply(PurchaseTaskUpdateDto purchaseTaskUpdate)
        {
            if (purchaseTaskUpdate.PurchaseTaskId != this.PurchaseTaskId) return;
            // if (purchaseTaskUpdate.PurchaseTaskVersion <= this.PurchaseTaskVersion) return;

            foreach(var lineUpdated in purchaseTaskUpdate.LinesUpdated)
            {
                var purchaseTaskLineIndex = -1;
                for (var i = 0; i < this._purchaseTaskLines.Count; i++)
                {
                    if (_purchaseTaskLines[i].Id == lineUpdated.Id)
                    {
                        purchaseTaskLineIndex = i;
                        break;   
                    }
                }
                var lineLocal = purchaseTaskLineIndex != -1 ? this._purchaseTaskLines[purchaseTaskLineIndex] : null;
                if (lineLocal != null)
                {
                    lineLocal.Quantity = lineUpdated.Quantity;
                    lineLocal.State.ExpirationDate = lineUpdated.State.ExpirationDate;
                    lineLocal.State.ExpirationDaysPlus = lineUpdated.State.ExpirationDaysPlus;
                    lineLocal.State.QtyNormal = lineUpdated.State.QtyNormal;
                    lineLocal.State.QtyBroken = lineUpdated.State.QtyBroken;
                    lineLocal.Product.Id = lineUpdated.Product.Id;
                    lineLocal.Product.Name = lineUpdated.Product.Name;
                    lineLocal.Product.Abc = lineUpdated.Product.Abc;
                    lineLocal.Product.Barcodes = lineUpdated.Product.Barcodes;
                    this._purchaseTaskLines.ResetItem(purchaseTaskLineIndex);
                }
                else
                {
                    this._purchaseTaskLines.Add(lineUpdated);
                }
            }
            foreach (var palletUpdated in purchaseTaskUpdate.PalletsUpdated)
            {
                var palletLocal = this._purchaseTaskPallets.FirstOrDefault(e => e.Code == palletUpdated.Code);
                if (palletLocal != null)
                {
                    palletLocal.ABC = palletUpdated.ABC;
                }
                else
                {
                    this._purchaseTaskPallets.Add(palletUpdated);
                }
            }

            this.PurchaseTaskVersion = purchaseTaskUpdate.PurchaseTaskVersion;
        }

        public BindingSource GetLinesDataSource()
        {
            _purchaseTaskLines.RaiseListChangedEvents = true;
            return new BindingSource { DataSource = _purchaseTaskLines };
        }
    }
}
