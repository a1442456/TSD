using System.Collections.Generic;
using System.Windows.Forms;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Datalogic.API;
using Microsoft.WindowsCE.Forms;
using NLog;

namespace Cen.Wms.Client.Forms.Common
{
    public class ScanBaseForm : Form
    {
        private List<int> requests;

        private WndMessageWindow wndMsg;
        private DecodeHandle hDcd;

        private static object uiLock;

        static ScanBaseForm()
        {
            uiLock = new object();
        }

        public ScanBaseForm()
        {
            try
            {
                requests = new List<int>();
                hDcd = new DecodeHandle(DecodeDeviceCap.Exists | DecodeDeviceCap.Barcode);
                wndMsg = new WndMessageWindow(this);
                AddRequest();
            }
            catch (DecodeException exception)
            {
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);
                ShowModalMessage.Run(Messages.TitleError, "Не удалось инициализировать сканер шртих-кодов!!!");
            }
        }

        protected virtual void ProcessBarcode(string barcode, CodeId codeId)
        {

        }

        public void ScanMsg(int msg, int reqID)
        {
            if (hDcd == null)
                return;

            lock (uiLock)
            {
                // An extra Scanned (with zero length data ) and 
                // Timeout message are sent after a request is cancelled.
                // Allow the Scanned call to ReadString to clear the
                // data box, but ignore the timeout.
                bool contains = requests.Contains(reqID);

                if (!contains)
                    return;

                try
                {
                    switch (msg)
                    {
                        case Constants.WM_SCANNED:
                            var barcode = hDcd.ReadString(reqID);
                            CleanRequests();
                            Beep.Run();
                            ProcessBarcode(barcode, CodeId.NoData);
                            break;
                        case Constants.WM_TIMEOUT:
                            hDcd.CancelRequest(reqID);
                            requests.Remove(reqID);

                            break;
                    }
                }
                finally
                {
                    RefreshRequests();
                }
            }
        }

        public void RefreshRequests()
        {
            if (requests.Count < 1)
                AddRequest();
        }

        public void CleanRequests()
        {
            while (requests.Count > 0)
            {
                hDcd.CancelRequest(requests[0]);
                requests.RemoveAt(0);
            }
        }

        // Post Request button
        public void AddRequest()
        {
            if (hDcd == null)
                return;

            int reqID = hDcd.PostRequestMsgEx((DecodeRequest)1, wndMsg, Constants.WM_SCANNED, Constants.WM_TIMEOUT);
            if (reqID != -1)
            {
                requests.Insert(requests.Count, reqID);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (hDcd != null))
            {
                CleanRequests();

                hDcd.Dispose();
                hDcd = null;
            }
            base.Dispose(disposing);
        }
    }

    public class WndMessageWindow : MessageWindow
    {
        // Reference to the window we will subclass.
        private ScanBaseForm dlgParent;

        public WndMessageWindow(ScanBaseForm frm)
        {
            this.dlgParent = frm;
        }

        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case Constants.WM_SCANNED:
                case Constants.WM_TIMEOUT:
                    dlgParent.ScanMsg(msg.Msg, msg.LParam.ToInt32());
                    break;
            }

            base.WndProc(ref msg);
        }

    }

    internal sealed class Constants
    {
        public const int WM_APP = 0x8000;
        public const int WM_SCANNED = WM_APP + 1;
        public const int WM_TIMEOUT = WM_APP + 2;
    }
}