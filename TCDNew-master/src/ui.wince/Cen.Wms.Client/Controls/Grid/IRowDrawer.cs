using System;
using System.Drawing;

namespace Cen.Wms.Client.Controls.Grid
{
    public interface IRowDrawer
    {
        void Init(int scale);

        int Rowheight { get; }

        void Draw(Graphics g, Object ndrv, int x, int y, bool selected);
    }
}
