using System.Linq;
using Cen.Wms.Client.Actions.Data.Purchase;
using Cen.Wms.Client.Actions.UI.Purchase;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Models.Dtos;
using Cen.Wms.Client.Services;

namespace Cen.Wms.Client.Actions.Operation
{
    class PurchaseTaskAcceptByPapers
    {
        public static void Run()
        {
            var purchasesHeads = PacHeadScan.Run();

            if (purchasesHeads != null)
            {
                if (purchasesHeads.Count > 0)
                {
                    var purchaseTaskId = PurchaseTaskCreateFromPacs.Run(
                        GStateProvider.Instance.SettingsFacility.FacilityId,
                        purchasesHeads.Select(ph => (PacKeyDto)ph)
                    );
                    if (purchaseTaskId != null)
                    {
                        PurchaseTaskContentScan.Run(purchaseTaskId);
                    }
                    else
                        ShowModalMessage.Run(Messages.TitleError, "Не удалось закрепить закупку за исполнителем!");
                }
                else
                    ShowModalMessage.Run(Messages.TitleError, "Закупки не выбраны!");
            }
        }
    }
}
