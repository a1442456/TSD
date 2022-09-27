using System;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Purchase;
using Cen.Wms.Client.Models.Dtos;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Purchase
{
    class PurchaseTaskLineEdit
    {
        public static PurchaseTaskLineUpdateDto Run(PurchaseTaskLineDto purchaseTaskLine)
        {
            PurchaseTaskLineUpdateDto result = null;
            Exception formException = null;

            try
            {
                using (var form = new PurchaseTaskLineEditForm(purchaseTaskLine))
                {
                    form.ShowDialog();
                    result = form.Result;
                }
            }
            catch (Exception exception)
            {
                formException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }
            if (formException != null)
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при обновлении информации по товарной позиции!");

            return result;
        }
    }
}
