using System;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Forms.Task;
using Cen.Wms.Client.Models.Enums;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Task
{
    class AskForTaskType
    {
        public static TaskType Run()
        {
            TaskType result = TaskType.None;
            DialogResult resultDialog = DialogResult.None;
            Exception formException = null;

            try
            {
                using (var form = new TaskSelectForm())
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
                ShowModalMessage.Run(Messages.TitleError, "Произошла ошибка при выборе задачи!");

            return resultDialog == DialogResult.OK ? result : TaskType.Exit;
        }
    }
}
