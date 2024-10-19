using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace AutoKitTest.Lib
{
    internal class ScreenChecker : IDisposable
    {
        private string _tempDir = Path.Combine(Item.WorkDir, "result");

        //private List<ImageItem> _templateImageItems = null;
        private bool _outputCheckedImage = false;
        private string _outputCheckedFilePath = null;




        private Bitmap _screenCapture = null;
        private Mat _screen = null;
        public ScreenChecker()
        {
            _screenCapture = ScreenCapture.FullScreen();
            _screen = BitmapConverter.ToMat(_screenCapture);
        }


        public ImageCheckResult LocateOnScreen(string name, string path, double threshold)
        {
            ImageCheckResult imageCheckResult = new();

            Console.WriteLine($"Screen mat type: {_screen.Type()}");

            using (Mat template = new(path, ImreadModes.Unchanged))
            using (Mat result = new Mat())
            {
                Console.WriteLine($"Template mat type: {template.Type()}");


                if (template.Type() == MatType.CV_8UC4)
                {
                    Cv2.CvtColor(template, template, ColorConversionCodes.BGRA2BGR);
                }
                if (template.Type() != MatType.CV_8UC3)
                {
                    template.ConvertTo(template, MatType.CV_8UC3);
                }
                Cv2.MatchTemplate(_screen, template, result, TemplateMatchModes.CCorrNormed);
                OpenCvSharp.Point minLoc, maxLoc;
                double minVal, maxVal;
                Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                if (maxVal >= threshold)
                {
                    imageCheckResult.Name = name;
                    imageCheckResult.IsMatched = true;
                    imageCheckResult.Location = maxLoc;
                    imageCheckResult.Size = template.Size();
                    imageCheckResult.RectAngle_X = maxLoc.X;
                    imageCheckResult.RectAngle_Y = maxLoc.Y;
                    imageCheckResult.RectAngle_Width = template.Width;
                    imageCheckResult.RectAngle_Height = template.Height;
                }
            }

            return imageCheckResult;
        }

        public void AddRect(ImageCheckResult imageCheckResult)
        {
            _screen.Rectangle(new Rect(imageCheckResult.Location, imageCheckResult.Size), Scalar.Red, 2);
            _screen.PutText(imageCheckResult.Name, imageCheckResult.Location, HersheyFonts.HersheyDuplex, 1, Scalar.Red);
        }

        public void SaveScreen(string path)
        {
            _screenCapture.Save(path, ImageFormat.Png);
        }

        #region Disposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _screenCapture != null)
                {
                    _screen.Dispose();
                    _screenCapture.Dispose();
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
