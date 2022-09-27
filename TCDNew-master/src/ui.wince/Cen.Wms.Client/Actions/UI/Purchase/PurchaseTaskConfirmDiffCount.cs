using System;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Purchase
{
    class PurchaseTaskConfirmDiffCount
    {
        public static bool Run(int diffCount)
        {
            var result = false;
            Exception modalDialogException = null;

            try
            {
                var dialogResult = ShowModalDialog.Run(Messages.TitleMessage, string.Format("Количество расхождений: {0}", diffCount));
                result = (dialogResult == DialogResult.OK);
            }
            catch (Exception exception)
            {
                modalDialogException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }

            if (modalDialogException != null)
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при подтверждении кол-ва расхождений!");

            return result;
        }
    }
}
