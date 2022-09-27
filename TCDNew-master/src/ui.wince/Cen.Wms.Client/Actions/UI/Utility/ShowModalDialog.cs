using System.Windows.Forms;
using Cen.Wms.Client.Forms.Utility;

namespace Cen.Wms.Client.Actions.UI.Utility
{
    class ShowModalDialog
    {
        public static DialogResult Run(string title, string message)
        {
            var result = DialogResult.None;
            using (var formDialog = new DialogForm())
            {
                formDialog.Text = title;
                formDialog.lblInfo.Text = message;
                result = formDialog.ShowDialog();
            }
            return result;
        }
    }
}
