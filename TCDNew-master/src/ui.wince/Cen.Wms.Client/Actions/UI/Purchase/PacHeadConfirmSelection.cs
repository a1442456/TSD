using System;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Purchase;
using Cen.Wms.Client.Models.Dtos;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Purchase
{
    class PacHeadConfirmSelection
    {
        public static bool Run(PacHeadDto pacHead)
        {
            var result = false;
            DialogResult resultDialog;
            Exception formException = null;

            try
            {
                using (var form = new PacConfirmSelectionForm(pacHead))
                {
                    resultDialog = form.ShowDialog();
                    result = resultDialog == DialogResult.OK;
                }
            }
            catch (Exception exception)
            {
                formException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }

            if (formException != null)
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при подтверждении выбора закупки!");

            return result;
        }
    }
}
