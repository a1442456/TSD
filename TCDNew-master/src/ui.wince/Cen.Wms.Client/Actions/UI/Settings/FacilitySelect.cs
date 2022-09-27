using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Settings;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Settings
{
    class FacilitySelect
    {
        public static string Run()
        {
            string results = null;
            DialogResult resultDialog = DialogResult.None;
            Exception formException = null;

            try
            {
                using (var form = new FacilitySelectForm())
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
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при выборе торгового объекта!");

            return resultDialog == DialogResult.OK ? results : null;
        }
    }
}
