using System;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Purchase;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Purchase
{
    class PurchaseTaskPause
    {
        public static DialogResult Run()
        {
            var result = DialogResult.None;
            Exception formException = null;

            try
            {
                using (var form = new PurchaseTaskPauseForm())
                {
                    result = form.ShowDialog();
                }
            }
            catch (Exception exception)
            {
                formException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }

            if (formException != null)
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при выборе операции!");

            return result;
        }
    }
}
