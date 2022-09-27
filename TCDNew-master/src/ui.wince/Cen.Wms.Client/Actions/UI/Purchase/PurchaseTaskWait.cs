using System;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Purchase;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Purchase
{
    class PurchaseTaskWait
    {
        public static string Run()
        {
            string results = null;
            DialogResult resultDialog = DialogResult.None;
            Exception formException = null;

            try
            {
                using (var form = new PurchaseTaskWaitForm())
                {
                    resultDialog = form.ShowDialog();
                    results = form.Result;
                }
            }
            catch (Exception exception)
            {
                formException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }

            if (formException != null)
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при выборе задания!");

            return resultDialog == DialogResult.OK ? results : null;
        }
    }
}
