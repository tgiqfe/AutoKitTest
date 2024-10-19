using System.Runtime.InteropServices;

namespace AutoKitTest.Lib
{
    internal class PInvoke
    {
        internal const int SRCCOPY = 13369376;
        internal const int CAPTUREBLT = 1073741824;

        [DllImport("user32.dll")]
        internal static extern nint GetDC(nint hwnd);

        [DllImport("gdi32.dll")]
        internal static extern int BitBlt(nint hDeskDC,
            int x,
            int y,
            int nWidth,
            int nHeight,
            nint hSrcDC,
            int xSrc,
            int ySrc,
            int dwRop);

        [DllImport("user32.dll")]
        internal static extern nint ReleaseDC(nint hwnd, nint hDC);

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        internal static extern nint GetWindowDC(nint hwnd);

        [DllImport("user32.dll")]
        internal static extern nint GetForegroundWindow();

        [DllImport("user32.dll")]
        internal static extern int GetWindowRect(nint hwnd, ref RECT lpRect);
    }
}
