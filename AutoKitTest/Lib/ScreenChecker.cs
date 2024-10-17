using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib
{
    internal class ScreenChecker : IDisposable
    {
        private string _tempDir = Path.Combine(Item.WorkDir, "result");
        private Bitmap _screenCapture = null;
        private List<ImageItem> _templateImageItems = null;
        private bool _outputCheckedImage = false;
        private string _outputCheckedFilePath = null;

        public ScreenChecker(List<ImageItem> templateImageItems, bool outputCheckedImage = false)
        {
            if (!Directory.Exists(_tempDir))
            {
                Directory.CreateDirectory(_tempDir);
            }

            _templateImageItems = templateImageItems;
            if (outputCheckedImage)
            {
                _outputCheckedImage = true;
                _outputCheckedFilePath = Path.Combine(
                    Item.WorkDir, "result", "result_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png");
            }

            _screenCapture = ScreenCapture.FullScreen();
        }

        public void LocateOnScreen()
        {
            using (Mat screen = BitmapConverter.ToMat(_screenCapture))
            {
                foreach (var imageItem in _templateImageItems)
                {
                    using (Mat template = new(imageItem.Path, ImreadModes.Unchanged))
                    using (Mat result = new Mat())
                    {
                        if (template.Type() == MatType.CV_8UC4)
                        {
                            Cv2.CvtColor(template, template, ColorConversionCodes.BGRA2BGR);
                        }
                        if (template.Depth() != MatType.CV_8UC3)
                        {
                            template.ConvertTo(template, MatType.CV_8UC3);
                        }

                        Console.WriteLine($"Template Depth: {template.Depth()}, Type: {template.Type()}");

                        Cv2.MatchTemplate(screen, template, result, TemplateMatchModes.CCorrNormed);
                        OpenCvSharp.Point minLoc, maxLoc;
                        double minVal, maxVal;
                        Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                        if (maxVal >= imageItem.Threshold)
                        {
                            imageItem.Location = maxLoc;
                            imageItem.Size = template.Size();

                            imageItem.RectAngle_X = maxLoc.X;
                            imageItem.RectAngle_Y = maxLoc.Y;
                            imageItem.RectAngle_Width = template.Width;
                            imageItem.RectAngle_Height = template.Height;
                        }
                    }
                }
                if (_outputCheckedImage)
                {
                    foreach (var imageItem in _templateImageItems)
                    {
                        if (imageItem.IsMatched == true)
                        {
                            screen.Rectangle(new Rect(imageItem.Location, imageItem.Size), Scalar.Lime, 2);
                            screen.PutText(imageItem.Name, imageItem.Location, HersheyFonts.HersheyDuplex, 1, Scalar.Lime);
                        }
                    }
                    screen.SaveImage(_outputCheckedFilePath);
                }
            }
        }

        #region Disposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _screenCapture != null)
                {
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
