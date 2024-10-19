using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace AutoKitTest.Lib
{
    internal class ScreenChecker : IDisposable
    {
        private Bitmap _screenCapture = null;
        private Mat _screen = null;

        public ScreenChecker()
        {
            GetScreenCapture();
        }

        private void GetScreenCapture()
        {
            nint desktopDC = PInvoke.GetDC(nint.Zero);
            _screenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppRgb);
            using (Graphics g = Graphics.FromImage(_screenCapture))
            {
                nint srcDC = g.GetHdc();
                PInvoke.BitBlt(srcDC, 0, 0, _screenCapture.Width, _screenCapture.Height, desktopDC, 0, 0, PInvoke.SRCCOPY);
                g.ReleaseHdc(srcDC);
                PInvoke.ReleaseDC(nint.Zero, srcDC);
            }
            PInvoke.ReleaseDC(nint.Zero, desktopDC);
            _screen = BitmapConverter.ToMat(_screenCapture);
        }

        public ImageCheckResult LocateOnScreen(string tag, string path, double threshold)
        {
            ImageCheckResult icresult = new();

            using (Mat template = new(path, ImreadModes.Unchanged))
            using (Mat result = new())
            {
                Console.WriteLine(tag + ": " + template.Type());

                if (template.Type() == MatType.CV_8UC4)
                {
                    Cv2.CvtColor(template, template, ColorConversionCodes.BGRA2BGR);
                }
                if (template.Type() == MatType.CV_8UC1)
                {
                    Cv2.CvtColor(template, template, ColorConversionCodes.GRAY2RGB);
                }
                if (template.Type() != MatType.CV_8UC3)
                {
                    template.ConvertTo(template, MatType.CV_8UC3);
                }

                Console.WriteLine(tag + ": " + template.Type());

                Cv2.MatchTemplate(_screen, template, result, TemplateMatchModes.CCorrNormed);
                OpenCvSharp.Point minLoc, maxLoc;
                double minVal, maxVal;
                Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                if (maxVal >= threshold)
                {
                    icresult.IsMatched = true;
                    icresult.Tag = tag;
                    icresult.Location = maxLoc;
                    icresult.Size = new OpenCvSharp.Size(template.Width, template.Height);
                }
            }
            return icresult;
        }

        public void AddRect(ImageCheckResult icresult)
        {
            if (icresult.IsMatched)
            {
                //  Draw rectangle
                _screen.Rectangle(new Rect(icresult.Location, icresult.Size), Scalar.Lime, 2);

                //  Draw text
                var point = icresult.Location;
                if (point.Y < 25)
                {
                    point.Y += 25;
                }
                _screen.PutText(icresult.Tag, point, HersheyFonts.HersheyDuplex, 1, Scalar.Lime);
            }
        }

        public void SaveScreen(string path)
        {
            _screen.ImWrite(path);
        }


        #region Disposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_screen != null) _screen.Dispose();
                    if (_screenCapture != null) _screenCapture.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
