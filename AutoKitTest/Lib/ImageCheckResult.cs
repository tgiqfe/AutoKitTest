namespace AutoKitTest.Lib
{
    internal class ImageCheckResult
    {
        public bool IsMatched { get; set; }

        public OpenCvSharp.Point Location { get; set; }
        public OpenCvSharp.Size Size { get; set; }
        public int RectAngle_X { get; set; }
        public int RectAngle_Y { get; set; }
        public int RectAngle_Width { get; set; }
        public int RectAngle_Height { get; set; }
    }
}
