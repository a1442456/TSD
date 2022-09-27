using System;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Purchase;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Purchase
{
    class PurchaseTaskContentScan
    {
        public static void Run(string purchaseTaskId)
        {
            Exception formException = null;

            try
            {
                using (var form = new PurchaseTaskContentScanForm(purchaseTaskId))
                {
                    form.ShowDialog();
                }
            }
            catch (Exception exception)
            {
                formException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }

            if (formException != null)
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при обработке закупки!!!");
        }
    }
}
