using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib
{
    internal class AutoGUI
    {
        private string _tempDir = Path.Combine(Item.WorkDir, "temp");
        private string _tempScreenCapturePath = Path.Combine(Item.WorkDir, "temp", "screencapture.png");

        public AutoGUI()
        {
            if (!Directory.Exists(_tempDir))
            {
                Directory.CreateDirectory(_tempDir);
            }
        }

        public bool LocateOnScreenFromImageItems(ImageItem[] tmplImageItems, out ImageItem[] results)
        {
            List<ImageItem> list = new();

            using (Bitmap screenCapture = ScreenCapture.FullScreen())
            using (Mat screen = BitmapConverter.ToMat(screenCapture))
            {
                foreach (var imageItem in tmplImageItems)
                {
                    using (Mat template = new Mat(imageItem.Path, ImreadModes.Unchanged))
                    using (Mat result = new Mat())
                    {
                        Cv2.MatchTemplate(screen, template, result, TemplateMatchModes.CCoeffNormed);
                        OpenCvSharp.Point minLoc, maxLoc;
                        double minVal, maxVal;
                        Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                        if (maxVal >= imageItem.Threshold)
                        {
                            imageItem.RectAngle_X = maxLoc.X;
                            imageItem.RectAngle_Y = maxLoc.Y;
                            imageItem.RectAngle_Width = template.Width;
                            imageItem.RectAngle_Height = template.Height;


                            screen.Rectangle(new Rect(maxLoc, template.Size()), Scalar.Lime, 2);
                            screen.PutText(imageItem.Name, maxLoc, HersheyFonts.HersheyDuplex, 1, Scalar.Lime);
                            screen.SaveImage(_tempScreenCapturePath);
                        }
                    }
                    list.Add(imageItem);
                }
            }

            results = list.ToArray();
            return false;
        }
    }
}
