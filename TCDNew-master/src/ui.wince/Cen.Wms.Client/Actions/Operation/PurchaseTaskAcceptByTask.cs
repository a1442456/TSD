using Cen.Wms.Client.Actions.UI.Purchase;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;

namespace Cen.Wms.Client.Actions.Operation
{
    class PurchaseTaskAcceptByTask
    {
        public static void Run()
        {
            var purchasesTaskId = PurchaseTaskWait.Run();
            if (purchasesTaskId != null)
            {
                PurchaseTaskContentScan.Run(purchasesTaskId);
            }
            else
                ShowModalMessage.Run(Messages.TitleError, "Задание не выбрано!");
        }
    }
}
