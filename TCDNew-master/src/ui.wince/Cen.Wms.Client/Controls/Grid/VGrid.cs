using System.Drawing;
using System.Windows.Forms;

namespace Cen.Wms.Client.Controls.Grid
{
    public class VGrid: ScrollControl
    {
        private readonly IRowDrawer _vrow;

        public VGrid(Control parent, BindingSource source, IRowDrawer row, int scale)
            : base(scale)
        {
            hs.Visible = false;
            _vrow = row;
            _vrow.Init(scale);
            itemHeight = _vrow.Rowheight;
            ItemsTop = 0;

            this.Bounds = new Rectangle() { Location = new Point { X = 0, Y = 0 }, Size = parent.Size };
            this.BindSource = source;
            this.Parent = parent;
        }
        

        protected override void OnPaint(PaintEventArgs e)
        {
            int itemTop = ItemsTop;
            int dx = -hs.Value;
            Graphics gOffScreen = Graphics.FromImage(offScreen);
            gOffScreen.Clear(Color.White);

            int n = Vscroll.Position;

            while (itemTop < offScreen.Height-13 && n < BindSource.Count)
            {
                _vrow.Draw(gOffScreen, BindSource[n], dx, itemTop, n == SelectedIndex);

                n++;
                itemTop += _vrow.Rowheight;
            }
            if (Vscroll.Visible)
                Vscroll.Draw(gOffScreen);
            e.Graphics.DrawImage(offScreen, 0, 0);
            gOffScreen.Dispose();
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //  ничего не рисуем
        }

    }
}
