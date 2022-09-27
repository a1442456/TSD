using System;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Settings;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Settings
{
    class SettingsEdit
    {
        public static bool Run()
        {
            bool result = false;
            DialogResult resultDialog;
            Exception formException = null;

            try
            {
                using (var form = new SettingsEditForm())
                {
                    resultDialog = form.ShowDialog();
                }

                switch (resultDialog)
                {
                    case DialogResult.Cancel:
                        result = false;
                        break;
                    case DialogResult.OK:
                        result = true;
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
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при изменении настроек!");

            return result;
        }
    }
}
