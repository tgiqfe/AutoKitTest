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

        public bool LocateOnScreen(string[] templateImagePaths)
        {
            
            using (Bitmap screenCapture = ScreenCapture.FullScreen())
            using (Mat screen = BitmapConverter.ToMat(screenCapture))
            {
                double threshold = 0.99;
                foreach (var imagePath in templateImagePaths)
                {
                    using (Mat template = new Mat(imagePath, ImreadModes.Unchanged))
                    using (Mat result = new Mat())
                    {
                        Cv2.MatchTemplate(screen, template, result, TemplateMatchModes.CCoeffNormed);
                        OpenCvSharp.Point minLoc, maxLoc;
                        double minVal, maxVal;
                        Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

                        return maxVal >= threshold;
                    }
                }
            }

            return false;
        }
    }
}
