using System;
using System.Drawing;
using Cen.Wms.Client.Models.Dtos;

namespace Cen.Wms.Client.Controls.Grid
{
    public class RowDrawerPurchaseTaskLine : IRowDrawer
    {
        private int _scale;

        private int _rowHeight;
        private int _rowWidth;
        
        private int lin1, lin2, height;
        private int quantX, expX;
        private int snameLen;

        private readonly Font font = new Font("Arial", 8, FontStyle.Regular);
        private readonly Font font1 = new Font("Arial", 9, FontStyle.Regular);
        private readonly Pen grayPen = new Pen(Color.LightGray);
        private readonly Pen blackPen = new Pen(Color.Black);
        private readonly SolidBrush blueBrush = new SolidBrush(Color.Blue);
        private readonly SolidBrush redBrush = new SolidBrush(Color.Red);
        private readonly SolidBrush greenBrush = new SolidBrush(Color.LawnGreen);
        private readonly SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
        private readonly SolidBrush whiteBrush = new SolidBrush(Color.White);
        private readonly SolidBrush blackBrush = new SolidBrush(Color.Black);

        private readonly SolidBrush magentaBrush = new SolidBrush(Color.Magenta);
        private readonly SolidBrush grayBrush = new SolidBrush(Color.LightGray);
        private readonly SolidBrush cyanBrush = new SolidBrush(Color.Cyan);

        public void Init(int scale)
        {
            _scale = scale;
            _rowHeight = 40*scale;
            _rowWidth = 240*scale;

            lin1 = 13 * scale;
            lin2 = 26 * scale;
            height = _rowHeight - 1;

            snameLen = 37;
            quantX = 89 * scale;
            expX = 165 * scale;
        }

        public int Rowheight
        {
            get { return _rowHeight; }
        }

        private void DrawQtyColorMark(Graphics g, Object ndrv, int mx, int my, int mwidth, int mheight)
        {
            var drv = (PurchaseTaskLineDto) ndrv;

            var currentBrush = whiteBrush;
            if (drv.Quantity == drv.State.QtyFull)
                currentBrush = greenBrush;
            else if (drv.Quantity > 0 && drv.State.QtyFull != 0)
                currentBrush = yellowBrush;
            else if (drv.Quantity > 0 && drv.State.QtyFull == 0)
                currentBrush = redBrush;

            g.FillRectangle(currentBrush, mx, my, mwidth, mheight);
        }

        private void DrawABCColorMark(Graphics g, Object ndrv, int mx, int my, int mwidth, int mheight)
        {
            var drv = (PurchaseTaskLineDto)ndrv;

            var currentBrush = whiteBrush;
            if (drv.Product.Abc == "a")
                currentBrush = grayBrush;
            else if (drv.Product.Abc == "b")
                currentBrush = cyanBrush;
            else if (drv.Product.Abc == "c")
                currentBrush = magentaBrush;

            g.FillRectangle(currentBrush, mx, my, mwidth, mheight);
        }

        public void Draw(Graphics g, Object ndrv, int x, int y, bool selected)
        {
            var drv = (PurchaseTaskLineDto)ndrv;

            int lin1Y = lin1 + y;
            int lin2Y = lin2 + y;
            int bottom = height + y;

            DrawQtyColorMark(g, drv, 0, y, 10, height);
            DrawABCColorMark(g, drv, _rowWidth - 10, y, 10, height);

            var txtBrush = selected ? redBrush : blueBrush;

            // Name
            string name = drv.Product.Name;
            string name1 = name;
            string name2 = "";
            if (name.Length > snameLen)
            {
                name1 = name.Substring(0, snameLen);
                name2 = name.Substring(snameLen);
            }
            g.DrawString(name1, font1, txtBrush, _scale, y);
            g.DrawString(name2, font1, txtBrush, _scale, lin1Y);

            // Barcode
            var barcode = drv.Product.Barcodes.Length > 0 ? drv.Product.Barcodes[0] : "НЕТ!!!";
            g.DrawString(barcode, font, blackBrush, _scale, lin2Y);

            // Quantity
            if (drv.Quantity != 0)
                g.DrawString(String.Format("{0:n3} / {1:n3}", drv.State.QtyFull, drv.Quantity), font, redBrush, quantX + 4, lin2Y);

            // ExpirationDate
            if (drv.State.ExpirationDate.HasValue)
                if (drv.State.ExpirationDate > Convert.ToDateTime("1901-01-01"))
                {
                    var dateToDraw = drv.State.ExpirationDate.Value.AddDays(drv.State.ExpirationDaysPlus);
                    g.DrawString(String.Format("{0:dd.MM.yyyy}", dateToDraw), font, blackBrush, expX + 4, lin2Y);
                }

            // Grid
            g.DrawLine(grayPen, 0, lin2Y, _rowWidth, lin2Y);
            g.DrawLine(grayPen, expX, lin2Y, expX, bottom);
            g.DrawLine(grayPen, quantX, lin2Y, quantX, bottom);
            g.DrawLine(blackPen, 0, bottom, _rowWidth, bottom);
        }
    }
}
