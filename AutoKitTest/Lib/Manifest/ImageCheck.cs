using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib.Manifest
{
    internal class ImageCheck
    {
        public string Name { get; set; }
        public double Threshold { get; set; }
        public string Fomula { get; set; }

        public Dictionary<string, ImageCheckItem> ImageCheckItems { get; set; }
    }
}
