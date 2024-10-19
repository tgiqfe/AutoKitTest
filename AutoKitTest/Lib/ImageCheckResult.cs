namespace AutoKitTest.Lib
{
    internal class ImageCheckResult
    {
        public bool IsMatched { get; set; }
        public string Tag { get; set; }
        public OpenCvSharp.Point Location { get; set; }
        public OpenCvSharp.Size Size { get; set; }
    }
}
