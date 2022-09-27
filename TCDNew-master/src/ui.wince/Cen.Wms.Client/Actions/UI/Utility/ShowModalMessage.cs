using Cen.Wms.Client.Forms.Utility;
using System.Drawing;
using Cen.Wms.Client.Models.Dtos.GosZNKAK;

namespace Cen.Wms.Client.Actions.UI.Utility
{
    class ShowModalMessage
    {
        public static void Run(string title, string message)
        {
            using (var formMessage = new MessageForm())
            {
                formMessage.Text = title;
                formMessage.lblInfo.Text = message;
                formMessage.ShowDialog();
            }
        }

        public static void Run(string title, string message, Color clr)
        {
            using (var formMessage = new MessageForm())
            {
                formMessage.Text = title;
                formMessage.lblInfo.BackColor = clr;
                formMessage.lblInfo.Text = message;
                formMessage.ShowDialog();
            }
        }

        public static void Run(string title, GosZNAKLabels lbl, Color clr)
        {
            using (var formMessage = new GosZnakMessageForm())
            {   
                formMessage.Text = title;
                formMessage.pnlGosZnakStatus.BackColor = clr;
                formMessage.lblItemGroupName.Text = lbl.item.group.name;
                formMessage.lblItemName.Text = lbl.item.name;
                formMessage.lblLabelTypeName.Text = lbl.label.type.name;
                formMessage.lblLabelStatusMessage.Text = lbl.label.status.message;
                formMessage.lblLabelSNomer.Text = lbl.label.snomer;
                formMessage.lblUserName.Text = lbl.user.name;
                formMessage.lblUserUNP.Text = lbl.user.unp;
                formMessage.lblItemGTIN.Text = lbl.item.gtin;
                formMessage.ShowDialog();
            }
        }
    }
}
