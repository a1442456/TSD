using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Purchase;
using Cen.Wms.Client.Models.Dtos;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Purchase
{
    class PacHeadScan
    {
        public static List<PacHeadDto> Run()
        {
            List<PacHeadDto> results = null;
            DialogResult resultDialog = DialogResult.None;
            Exception formException = null;

            try
            {
                using (var form = new PacScanForm())
                {
                    resultDialog = form.ShowDialog();
                    results = form.Results;
                }
            }
            catch (Exception exception)
            {
                formException = exception;
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
            }

            if (formException != null)
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при сканировании штрих-кода закупки!");

            return resultDialog == DialogResult.OK ? results : null;
        }
    }
}
