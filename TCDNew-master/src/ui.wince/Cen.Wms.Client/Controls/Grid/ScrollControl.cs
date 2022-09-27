using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Cen.Wms.Client.Controls.Grid
{
    public class ScrollControl : Control
    {
        private int _scrollWidth;
        protected BindingSource bindingSource;
        protected int hdist;
        protected HScrollBar hs;
        protected int itemHeight = -1;
        protected Bitmap offScreen;
        private int _selectedIndex = -1;
        protected Scroller Vscroll;
        protected int _scale;
        protected int ItemsTop;

        public ScrollControl(int scale)
        {
            _scale = scale;
            Vscroll = new Scroller(scale);
            Vscroll.VScrollPositionChanged += vscroll_VScrollPositionChanged;
            hs = new HScrollBar
            {
                Parent = this,
                Visible = false,
                SmallChange = 15 * scale,
                LargeChange = 50 * scale,
                Maximum = 208 * scale
            };
            hs.ValueChanged += HScrollValueChanged;
            _scrollWidth = 18 * scale;

        }

        void vscroll_VScrollPositionChanged(object sender, EventArgs e)
        {
            Invalidate();

        }



        public int SelectedIndex
        {
            get { return _selectedIndex; }

            set
            {
                _selectedIndex = value;
                BindSource.Position = SelectedIndex;

                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        public BindingSource BindSource
        {
            get
            {
                if (bindingSource == null) bindingSource = new BindingSource();
                return bindingSource;
            }

            set
            {
                bindingSource = value;
                bindingSource.ListChanged += bindingSource_ListChanged;
                bindingSource_ListChanged(this, null);
            }
        }

        protected int ItemHeight
        {
            get { return itemHeight; }

            set { itemHeight = value; }
        }

        protected int DrawCount
        {
            get
            {
                if (Vscroll.Position + Vscroll.LargeChange > Vscroll.Maximum)
                    return Vscroll.Maximum - Vscroll.Position + 2;
                return Vscroll.LargeChange + 1;
            }
        }

        private void bindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {

            if (bindingSource == null) return;
            Vscroll.Maximum = bindingSource.Count;
            int viewableItemCount = ClientSize.Height / ItemHeight - 1;
            if (bindingSource.Count > viewableItemCount)
            {
                Vscroll.Visible = true;
                Vscroll.LargeChange = viewableItemCount;
                if (Vscroll.Position > bindingSource.Count)
                    Vscroll.Position = bindingSource.Count - 2;
            }
            else
            {
                Vscroll.Visible = false;
                Vscroll.LargeChange = bindingSource.Count;
                Vscroll.Position = 0;
            }
            Invalidate();
        }

        public event EventHandler SelectedIndexChanged;

        public virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Vscroll.Bounds.Contains(e.X, e.Y) && Vscroll.Visible)
            {
                Vscroll.MouseDown(e);
            }
            else
            {
                SelectedIndex = Vscroll.Position + (e.Y - ItemsTop) / itemHeight;
                //               BindSource.Position = SelectedIndex;
                //               OnSelectedIndexChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Vscroll.MouseUp(e);
        }



        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Vscroll.Bounds.Contains(e.X, e.Y) && Vscroll.Visible)
            {
                Vscroll.MouseMove(e);
            }

        }


        protected void HScrollValueChanged(object o, EventArgs e)
        {
            hdist = hs.Value;
            Refresh();
        }



        protected override void OnKeyDown(KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.Down:
            //        if (SelectedIndex < vscroll.Maximum)
            //        {
            //            EnsureVisible(++SelectedIndex);
            //            Refresh();
            //        }
            //        break;
            //    case Keys.Up:
            //        if (SelectedIndex > vscroll.Minimum)
            //        {
            //            EnsureVisible(--SelectedIndex);
            //            Refresh();
            //        }
            //        break;
            //    case Keys.PageDown:
            //        SelectedIndex = Math.Min(vscroll.Maximum, SelectedIndex + DrawCount);
            //        EnsureVisible(SelectedIndex);
            //        Refresh();
            //        break;
            //    case Keys.PageUp:
            //        SelectedIndex = Math.Max(vscroll.Minimum, SelectedIndex - DrawCount);
            //        EnsureVisible(SelectedIndex);
            //        Refresh();
            //        break;
            //    case Keys.Home:
            //        SelectedIndex = 0;
            //        EnsureVisible(SelectedIndex);
            //        Refresh();
            //        break;
            //    case Keys.End:
            //        SelectedIndex = bindingSource.Count - 1;
            //        EnsureVisible(SelectedIndex);
            //        Refresh();
            //        break;
            //}

            //base.OnKeyDown(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (offScreen == null)
            {
                //hs.Bounds = new Rectangle(0, ClientSize.Height - 13 * _scale,
                //                          ClientSize.Width - 16 * _scale, 13 * _scale);

                Vscroll.Bounds = new Rectangle(ClientSize.Width - _scrollWidth,
                                               ItemsTop,
                                               _scrollWidth,
                                               ClientSize.Height - ItemsTop);
                offScreen = new Bitmap(ClientSize.Width, ClientSize.Height);
            }
        }
    }

}
