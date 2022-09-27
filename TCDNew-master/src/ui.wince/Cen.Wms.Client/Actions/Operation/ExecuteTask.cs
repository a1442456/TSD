using Cen.Wms.Client.Actions.UI.Task;
using Cen.Wms.Client.Models.Enums;

namespace Cen.Wms.Client.Actions.Operation
{
    class ExecuteTask
    {
        public static void Run()
        {
            var taskType = TaskType.None;
            while (taskType != TaskType.Exit)
            {
                taskType = AskForTaskType.Run();

                if (taskType == TaskType.PurchaseByPapers)
                    PurchaseTaskAcceptByPapers.Run();

                if (taskType == TaskType.PurchaseByTask)
                    PurchaseTaskAcceptByTask.Run();
            }
        }
    }
}
