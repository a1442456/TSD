using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Cen.Wms.Client.Controls
{
    public static class XGdiEx
    {
        public struct BlendFunction
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        public enum BlendOperation : byte
        {
            AC_SRC_OVER = 0x00
        }

        public enum BlendFlags : byte
        {
            Zero = 0x00
        }

        [DllImport("coredll.dll")]
        extern public static Boolean BitBlt(
            IntPtr hdcDest,
            Int32 nXDest,
            Int32 nYDest,
            Int32 nWidth,
            Int32 nHeight,
            IntPtr hdcSrc,
            Int32 nXSrc,
            Int32 nYSrc,
            UInt32 dwRop);


        [DllImport("coredll.dll")]
        extern public static Int32 AlphaBlend(
            IntPtr hdcDest,
            Int32 xDest,
            Int32 yDest,
            Int32 cxDest,
            Int32 cyDest,
            IntPtr hdcSrc,
            Int32 xSrc,
            Int32 ySrc,
            Int32 cxSrc,
            Int32 cySrc,
            BlendFunction blendFunction);

        [StructLayout(LayoutKind.Sequential)]
        private struct TRIVERTEX
        {
            public int X;
            public int Y;
            public ushort Red;
            public ushort Green;
            public ushort Blue;
            public ushort Alpha;

            public TRIVERTEX(int x, int y, Color color)
            {
                X = x;
                Y = y;
                Red = (ushort)(color.R << 8);
                Green = (ushort)(color.G << 8);
                Blue = (ushort)(color.B << 8);
                Alpha = (ushort)(color.A << 8);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct GRADIENT_RECT
        {
            public uint UpperLeft;
            public uint LowerRight;

            public GRADIENT_RECT(uint upperLeft, uint lowerRight)
            {
                UpperLeft = upperLeft;
                LowerRight = lowerRight;
            }
        }

        [DllImport("coredll.dll")]
        extern private static Int32 GradientFill(
            IntPtr hdc, TRIVERTEX[] pVertex,
            uint dwNumVertex,
            GRADIENT_RECT[] pMesh,
            uint dwNumMesh,
            uint dwMode);

        const int GRADIENT_FILL_RECT_H = 0x00000000;
        const int GRADIENT_FILL_RECT_V = 0x00000001;

        // The direction to the GradientFill will follow
        public enum FillDirection
        {
            LeftToRight = GRADIENT_FILL_RECT_H,
            TopToBottom = GRADIENT_FILL_RECT_V
        }

        public static void GradientFill(this Graphics g, Rectangle rect, Color startColor, Color endColor, FillDirection fillDir)
        {
            TRIVERTEX[] vertex = new TRIVERTEX[]
            {
                new TRIVERTEX(rect.X,rect.Y,startColor),
                new TRIVERTEX(rect.Right,rect.Bottom,endColor)
            };
            GRADIENT_RECT[] grect = new GRADIENT_RECT[] {
                new GRADIENT_RECT(0,1)
            };

            IntPtr hdc = g.GetHdc();
            GradientFill(hdc, vertex, (uint)vertex.Length, grect, (uint)grect.Length, (uint)fillDir);
            g.ReleaseHdc(hdc);
        }

        public static void AlphaBlendImage(this Graphics g, Image image, int x, int y, int alpha)
        {
            int width = image.Width;
            int weight = image.Height;
            using (Graphics imageG = Graphics.FromImage(image))
            {
                IntPtr hdcDst = g.GetHdc();
                IntPtr hdcSrc = imageG.GetHdc();
                BlendFunction blendFunction = new BlendFunction();
                blendFunction.BlendOp = (byte)BlendOperation.AC_SRC_OVER;
                blendFunction.BlendFlags = (byte)BlendFlags.Zero;
                blendFunction.SourceConstantAlpha = (byte)alpha;
                blendFunction.AlphaFormat = (byte)0;
                AlphaBlend(hdcDst, x, y, width, weight, hdcSrc, 0, 0, width, weight, blendFunction);
                imageG.ReleaseHdc(hdcSrc);
                g.ReleaseHdc(hdcDst);
            }
        }


        public static void ScrollUp(Image image, int dy)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                IntPtr hdc = g.GetHdc();
                try
                {
                    BitBlt(hdc, 0, dy, image.Width, image.Height - dy, hdc, 0, 0, SRCCOPY);
                }
                finally
                {
                    g.ReleaseHdc(hdc);
                }
            }
        }
        const UInt32 SRCCOPY = 13369376;

        public static void ScrollDown(Image image, int dy)
        {

            using (Graphics g = Graphics.FromImage(image))
            {
                IntPtr hdc = g.GetHdc();
                try
                {
                    BitBlt(hdc, 0, 0, image.Width, image.Height - dy, hdc, 0, dy, SRCCOPY);
                }
                finally
                {
                    g.ReleaseHdc(hdc);
                }
            }
        }

        public static void Copy(Graphics src, Rectangle srcBounds, Graphics dst, int dstX, int dstY)
        {
            IntPtr hdcSrc = src.GetHdc();
            IntPtr hdcDst = dst.GetHdc();
            try
            {
                BitBlt(hdcDst, dstX, dstY, srcBounds.Width, srcBounds.Height, hdcSrc, srcBounds.X, srcBounds.Y, SRCCOPY);
            }
            finally
            {
                dst.ReleaseHdc(hdcDst);
                src.ReleaseHdc(hdcSrc);
            }
        }


        public static void DrawStringShadow(Graphics g, string text, Font font, Color textColor, Color shadowColor, Rectangle bounds, StringFormat stringFormat)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Rectangle r = bounds;
                r.Height--;
                r.Width--;
                RectangleF tr = new RectangleF((float)r.Left, (float)r.Top, (float)r.Width, (float)r.Height);
                tr.X += 1f;
                tr.Y += 1f;
                using (Brush pen = new SolidBrush(shadowColor))
                {
                    g.DrawString(text, font, pen, tr, stringFormat);
                }
                tr.X -= 1f;
                tr.Y -= 1f;
                using (Brush pen = new SolidBrush(textColor))
                {
                    g.DrawString(text, font, pen, tr, stringFormat);
                }
            }
        }
        private static Int32 FILE_DEVICE_HAL = 0x00000101;
        private static Int32 FILE_ANY_ACCESS = 0x0;
        private static Int32 METHOD_BUFFERED = 0x0;

        [DllImport("coredll.dll")]
        private static extern bool KernelIoControl(int IoControlCode, IntPtr
          InputBuffer, Int32 InputBufferSize, byte[] OutputBuffer, int
          OutputBufferSize, ref Int32 BytesReturned);

        private static int IOCTL_HAL_GET_DEVICEID =
            ((FILE_DEVICE_HAL) << 16) | ((FILE_ANY_ACCESS) << 14)
             | ((21) << 2) | (METHOD_BUFFERED);

        public static string GetDeviceID()
        {
            byte[] OutputBuffer = new byte[256];
            int OutputBufferSize, BytesReturned;
            OutputBufferSize = OutputBuffer.Length;
            BytesReturned = 0;

            // Call KernelIoControl passing the previously defined
            // IOCTL_HAL_GET_DEVICEID parameter
            // We don’t need to pass any input buffers to this call
            // so InputBuffer and InputBufferSize are set to their null
            // values
            bool retVal = KernelIoControl(IOCTL_HAL_GET_DEVICEID,
                    IntPtr.Zero,
                    0,
                    OutputBuffer,
                    OutputBufferSize,
                    ref BytesReturned);

            // If the request failed, exit the method now
            if (retVal == false)
            {
                return null;
            }

            // Examine the OutputBuffer byte array to find the start of the 
            // Preset ID and Platform ID, as well as the size of the
            // PlatformID. 
            // PresetIDOffset – The number of bytes the preset ID is offset
            //                  from the beginning of the structure
            // PlatformIDOffset - The number of bytes the platform ID is
            //                    offset from the beginning of the structure
            // PlatformIDSize - The number of bytes used to store the
            //                  platform ID
            // Use BitConverter.ToInt32() to convert from byte[] to int
            int PresetIDOffset = BitConverter.ToInt32(OutputBuffer, 4);
            int PlatformIDOffset = BitConverter.ToInt32(OutputBuffer, 0xc);
            int PlatformIDSize = BitConverter.ToInt32(OutputBuffer, 0x10);

            // Convert the Preset ID segments into a string so they can be 
            // displayed easily.
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0:X8}-{1:X4}-{2:X4}-{3:X4}-",
                 BitConverter.ToInt32(OutputBuffer, PresetIDOffset),
                 BitConverter.ToInt16(OutputBuffer, PresetIDOffset + 4),
                 BitConverter.ToInt16(OutputBuffer, PresetIDOffset + 6),
                 BitConverter.ToInt16(OutputBuffer, PresetIDOffset + 8)));

            // Break the Platform ID down into 2-digit hexadecimal numbers
            // and append them to the Preset ID. This will result in a 
            // string-formatted Device ID
            for (int i = PlatformIDOffset; i < PlatformIDOffset + PlatformIDSize; i++)
            {
                sb.Append(String.Format("{0:X2}", OutputBuffer[i]));
            }

            // return the Device ID string
            return sb.ToString();
        }
    }

}
