using System;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Purchase;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Purchase
{
    class BarcodePalletScan
    {
        public static string Run()
        {
            string result = null;
            DialogResult resultDialog;
            Exception formException = null;

            try
            {
                using (var form = new BarcodePalletScanForm())
                {
                    resultDialog = form.ShowDialog();
                    result = form.Result;
                }

                switch (resultDialog)
                {
                    case DialogResult.Cancel:
                        result = null;
                        break;
                    case DialogResult.OK:
                        break;
                }
            }
            catch (Exception exception)
            {
                formException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }
            if (formException != null)
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при вводе штрих-кода!");

            return result;
        }
    }
}
