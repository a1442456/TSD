using System;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Auth;
using Cen.Wms.Client.Models.Dtos;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Auth
{
    class AskForUserCredentials
    {
        public static UserCredentials Run()
        {
            UserCredentials result = null;
            DialogResult resultDialog = DialogResult.None;
            Exception formException = null;

            try
            {
                using (var form = new UserCredentialsForm())
                {
                    resultDialog = form.ShowDialog();
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
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при вводе имени пользователя и пароля!");

            return resultDialog == DialogResult.OK ? result : null;
        }
    }
}
