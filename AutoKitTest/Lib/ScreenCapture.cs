using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib
{
    internal class ScreenCapture
    {
        #region PInvoke

        const int SRCCOPY = 13369376;
        const int CAPTUREBLT = 1073741824;

        [DllImport("user32.dll")]
        static extern nint GetDC(nint hwnd);

        [DllImport("gdi32.dll")]
        static extern int BitBlt(nint hDeskDC,
            int x,
            int y,
            int nWidth,
            int nHeight,
            nint hSrcDC,
            int xSrc,
            int ySrc,
            int dwRop);

        [DllImport("user32.dll")]
        static extern nint ReleaseDC(nint hwnd, nint hDC);

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        static extern nint GetWindowDC(nint hwnd);

        [DllImport("user32.dll")]
        static extern nint GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowRect(nint hwnd, ref RECT lpRect);

        #endregion

        
        /// <summary>
        /// Screenshot full display
        /// </summary>
        /// <returns></returns>
        public static Bitmap FullScreen()
        {
            Bitmap result = null;
            nint desktopDC = GetDC(nint.Zero);
            using (Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppRgb))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                nint srcDC = g.GetHdc();
                BitBlt(srcDC, 0, 0, bitmap.Width, bitmap.Height, desktopDC, 0, 0, SRCCOPY);

                g.ReleaseHdc(srcDC);
                ReleaseDC(nint.Zero, srcDC);
                result = bitmap.Clone() as Bitmap;
            }
            ReleaseDC(nint.Zero, desktopDC);

            return result;
        }

        /// <summary>
        /// Screenshot active window
        /// </summary>
        /// <returns></returns>
        public static Bitmap ActiveWindow()
        {
            Bitmap result = null;
            nint hWnd = GetForegroundWindow();
            nint windowDC = GetWindowDC(hWnd);
            RECT rect = new RECT();
            GetWindowRect(hWnd, ref rect);

            using (Bitmap bitmap = new Bitmap(rect.Right - rect.Left, rect.Bottom - rect.Top))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                nint srcDC = g.GetHdc();
                BitBlt(srcDC, 0, 0, bitmap.Width, bitmap.Height, windowDC, 0, 0, SRCCOPY);

                g.ReleaseHdc(srcDC);
                ReleaseDC(nint.Zero, srcDC);
                result = bitmap.Clone() as Bitmap;
            }
            ReleaseDC(nint.Zero, windowDC);

            return result;
        }
    }
}
