using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib
{
    internal class ImageItem
    {
        public string Name { get { return System.IO.Path.GetFileName(this.Path); } }
        public string Path { get; set; }
        public double Threshold { get; set; }

        public int RectAngle_X { get; set; }
        public int RectAngle_Y { get; set; }
        public int RectAngle_Width { get; set; }
        public int RectAngle_Height { get; set; }
    }
}
