using System.Runtime.InteropServices;

namespace Cen.Wms.Client.Actions.UI.Utility
{
    class Beep
    {
        [DllImport("CoreDll.dll")]
        private static extern void MessageBeep(int code);

        public static void Run()
        {
            MessageBeep(-1);  // Default beep code is -1
        }
    }
}
