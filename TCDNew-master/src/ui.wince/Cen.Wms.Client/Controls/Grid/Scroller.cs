using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cen.Wms.Client.Controls.Grid
{
    public class Scroller
    {
        private Rectangle bounds;
        private Rectangle smallIncrRect;
        private Rectangle smallDecrRect;
        private Rectangle largeIncrRect;
        private Rectangle largeDecrRect;
        private Rectangle thumbRect;
        private Bitmap image;
        private Point[] uparrow;
        private Point[] downarrow;
        private int position;

        private int thumbY;
        private int thumbPix;
        private int thumbH;

        private int maximum;
        private int minimum;
        private int largechange;
        private int smallchange;

        private int _mousex;
        private int _mousey;

        private Color scolor;
        private SolidBrush sbrush;
        private bool visible;

        private ScrollBarAction _sba;
        private Timer _timer;
        private int _scale;
        private SolidBrush dbBrush = new SolidBrush(Color.OrangeRed);

        public Scroller(int scale)
        {
            //image = new Bitmap(10,10);
            scolor = Color.RoyalBlue;
            sbrush = new SolidBrush(scolor);
            minimum = 0;
            maximum = 1;
            smallchange = 1;
            _sba = ScrollBarAction.None;
            _scale = scale;
        }


        public Rectangle Bounds
        {
            get { return bounds; }
            set
            {
                bounds = value;
                image = new Bitmap(bounds.Width, bounds.Height);
                uparrow = new[] { new Point(2, bounds.Width - 2), new Point(bounds.Width - 2, bounds.Width - 2), new Point(bounds.Width / 2, 2) };
                downarrow = new[] { new Point(2, bounds.Height - bounds.Width + 2), new Point(bounds.Width - 2, bounds.Height - bounds.Width + 2), new Point(bounds.Width / 2, bounds.Height - 2) };
                smallIncrRect = SmallIncrementRectangle();
                smallDecrRect = SmallDecrementRectangle();
            }
        }

        public event EventHandler VScrollPositionChanged;

        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                SyncThumbPositionWithLogicalValue();

            }
        }

        public int LargeChange
        {
            get { return largechange; }
            set
            {
                largechange = value;
                Double domain = maximum - minimum + 1;
                int th = (Int32)((image.Height - (image.Width + 3) * 2) / (domain / largechange));
                thumbH = th < 16 * _scale ? 16 * _scale : th;
                thumbPix = image.Height - image.Width * 2 - thumbH;
                SyncThumbPositionWithLogicalValue();
            }
        }

        public int Maximum
        {
            get { return maximum; }
            set { maximum = value - 2; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public int Minimum
        {
            get { return minimum; }
            set { minimum = value; }
        }

        public int SmallChange
        {
            get { return smallchange; }
            set { smallchange = value; }
        }

        public void Draw(Graphics gx)
        {
            Graphics gimage = Graphics.FromImage(image);
            gimage.Clear(Color.White);
            gimage.FillRectangle(sbrush, 0, 0, bounds.Width, bounds.Width + 3);
            gimage.FillRectangle(sbrush, 0, bounds.Height - bounds.Width - 3, bounds.Width, bounds.Width + 3);

            gimage.FillPolygon(dbBrush, uparrow);
            gimage.FillPolygon(dbBrush, downarrow);

            gimage.FillRectangle(sbrush, 0, thumbY, bounds.Width, thumbH);
            gimage.FillRectangle(dbBrush, 4 * _scale, thumbY + thumbH / 2 - 4 * _scale, 8 * _scale, 8 * _scale);

            gx.AlphaBlendImage(image, bounds.X, bounds.Y, 128);
        //    gx.DrawImage(image, bounds.X, bounds.Y);

            gimage.Dispose();
        }

        public void MouseDown(MouseEventArgs e)
        {
            Rectangle r = ThumbRectangle;
            if (r.Contains(e.X, e.Y))
            {
                _mousey = e.Y - r.Top;
                _sba = ScrollBarAction.ThumbTrack;
                return;
            }
            r = smallDecrRect;
            if (r.Contains(e.X, e.Y))
            {

                _sba = ScrollBarAction.SmallDecrement;

                if (position != minimum)
                {

                    SmallDecrement();

                    StartTimer();

                }
                return;
            }
            r = smallIncrRect;
            if (r.Contains(e.X, e.Y))
            {

                _sba = ScrollBarAction.SmallIncrement;

                if (position != maximum)
                {

                    SmallIncrement();

                    StartTimer();

                }
                return;
            }
            r = LargeDecrementRectangle;
            if (r.Contains(e.X, e.Y))
            {

                _sba = ScrollBarAction.LargeDecrement;
                _mousex = e.X;
                _mousey = e.Y;

                if (position != minimum)
                {

                    LargeDecrement();

                    StartTimer();

                }
                return;
            }
            r = LargeIncrementRectangle;
            if (r.Contains(e.X, e.Y))
            {

                _sba = ScrollBarAction.LargeIncrement;
                _mousex = e.X;
                _mousey = e.Y;

                if (position != maximum)
                {

                    LargeIncrement();

                    StartTimer();

                }
            }

        }

        public void MouseMove(MouseEventArgs e)
        {
            if (_sba == ScrollBarAction.ThumbTrack)
            {
                int yp = (e.Y - _mousey) - smallDecrRect.Height - bounds.Y;

                thumbY = yp;
                if (yp < 0)
                    thumbY = 0;
                if (yp > thumbPix)
                    thumbY = thumbPix;


                Double domain = maximum - minimum;
                Position = (int)(thumbY / (double)thumbPix * domain);

            }
        }

        public void MouseUp(MouseEventArgs e)
        {
            _sba = ScrollBarAction.None;
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        void StartTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer();
                _timer.Tick += new EventHandler(OnTimerTick);
            }
            _timer.Interval = 250;
            _timer.Enabled = true;
        }

        void OnTimerTick(Object sender, EventArgs e)
        {
            if (_timer.Interval == 250)
                _timer.Interval = 50;

            if (_sba == ScrollBarAction.SmallDecrement)
            {
                if (position != minimum)
                {
                    SmallDecrement();
                }
            }
            else if (_sba == ScrollBarAction.SmallIncrement)
            {
                if (position != maximum)
                {
                    SmallIncrement();
                }
            }
            else if (_sba == ScrollBarAction.LargeDecrement)
            {
                //Rectangle r = LargeDecrementRectangle();
                //if (r.Contains(_mousex, _mousey))
                //{
                if (position != minimum)
                {
                    LargeDecrement();
                }
                //}
            }
            else if (_sba == ScrollBarAction.LargeIncrement)
            {
                //Rectangle r = LargeIncrementRectangle();
                //if (r.Contains(_mousex, _mousey))
                //{
                if (position != maximum)
                {
                    LargeIncrement();
                }
                //}
            }
        }
        private void SmallIncrement()
        {
            int newvalue = position + smallchange;
            if (newvalue > maximum)
                newvalue = maximum;
            Position = newvalue;
        }

        private void SmallDecrement()
        {
            int newvalue = position - smallchange;
            if (newvalue < minimum)
                newvalue = minimum;
            Position = newvalue;
        }

        private void LargeIncrement()
        {
            int newvalue = position + largechange;
            if (newvalue > maximum)
                newvalue = maximum;
            Position = newvalue;
        }

        private void LargeDecrement()
        {
            int newvalue = position - largechange;
            if (newvalue < minimum)
                newvalue = minimum;
            Position = newvalue;
        }
        private void SyncThumbPositionWithLogicalValue()
        {
            double domain = maximum - minimum;
            if (domain > 0)
                thumbY = (int)(position / (double)domain * thumbPix) + bounds.Width;
            else
                thumbY = 0;
            if (VScrollPositionChanged != null)
                VScrollPositionChanged(this, null);
        }



        private Rectangle SmallDecrementRectangle()
        {
            int x, y, w, h;

            x = bounds.X;
            y = bounds.Y;
            w = bounds.Width;
            h = bounds.Width;

            return new Rectangle(x, y, w, h);
        }

        private Rectangle SmallIncrementRectangle()
        {
            int x, y, w, h;

            x = bounds.X;
            h = bounds.Width;
            w = bounds.Width;
            y = bounds.Bottom - h;

            return new Rectangle(x, y, w, h);
        }

        private Rectangle LargeDecrementRectangle
        {
            get
            {
                if (largeDecrRect == null)
                    largeDecrRect = new Rectangle();
                largeDecrRect.X = bounds.X;
                largeDecrRect.Y = bounds.Width + bounds.Y;
                largeDecrRect.Width = bounds.Width;
                largeDecrRect.Height = thumbY - bounds.Width;
                return largeDecrRect;
            }
        }

        private Rectangle LargeIncrementRectangle
        {
            get
            {
                if (largeIncrRect == null)
                    largeIncrRect = new Rectangle();

                largeIncrRect.X = bounds.X;
                largeIncrRect.Y = thumbY + thumbH + bounds.Y;
                largeIncrRect.Width = bounds.Width;
                largeIncrRect.Height = bounds.Height - bounds.Width - (thumbY + thumbH);

                return largeIncrRect;
            }
        }

        private Rectangle ThumbRectangle
        {
            get
            {
                if (thumbRect == null)
                    thumbRect = new Rectangle();

                thumbRect.X = bounds.X;
                thumbRect.Width = bounds.Width;
                thumbRect.Y = bounds.Y + thumbY;
                thumbRect.Height = thumbH;

                return thumbRect;
            }
        }
    }


    public enum ScrollBarAction
    {
        None,
        SmallDecrement,
        LargeDecrement,
        ThumbTrack,
        LargeIncrement,
        SmallIncrement
    }

}
