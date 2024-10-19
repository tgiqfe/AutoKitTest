using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace AutoKitTest.Lib.Manifest
{
    internal class CommandImageCheck
    {
        public string Name { get; set; }
        public int? Timeout { get; set; }
        public int? Interval { get; set; }
        public double? Threshould { get; set; }
        public string Fomula { get; set; }
        public List<ImageItem> ImageItems { get; set; }

        private static Regex _sufPattern = new Regex(@",\s*[\d\.]+$");
        const int _defaultTimeout = 10000;
        const int _defaultInterval = 1000;
        const double _defaultThreshould = 0.98;

        public CommandImageCheck(TestCommand command)
        {
            this.Name = command.Name;
            this.Timeout = command.Timeout ?? _defaultTimeout;
            this.Interval = command.Interval ?? _defaultInterval;
            this.Threshould = command.Threshould ?? _defaultThreshould;
            this.Fomula = command.Fomula;

            this.ImageItems = command.ImageCheck.Select(x =>
            {
                if (!x.Contains(",")) { return null; }

                string tag = x.Substring(0, x.IndexOf(","));
                string path = x.Substring(x.IndexOf(",") + 1).Trim();
                if (_sufPattern.IsMatch(path))
                {
                    string suf = _sufPattern.Match(path).Value;
                    path = path.Substring(0, path.Length - suf.Length).Trim();
                    double threshold = double.Parse(suf.TrimStart(',').Trim());
                    return new ImageItem() { Tag = tag, Path = path, Threshold = threshold };
                }
                else
                {
                    return new ImageItem() { Tag = tag, Path = path, Threshold = (double)this.Threshould };
                }
            }).Where(x => x != null).ToList();
        }

        public bool Execute()
        {
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalMilliseconds < (this.Timeout ?? _defaultTimeout))
            {
                Console.WriteLine(DateTime.Now.ToString("[yyyyM/MM/dd HH:mm:ss]") + " " + this.Name + " is checking.");
                using (var checker = new ScreenChecker())
                {
                    foreach (var item in this.ImageItems)
                    {
                        var imageCheckResult = checker.LocateOnScreen(this.Name, item.Path, item.Threshold);
                        this.Fomula = this.Fomula.Replace("{" + item.Tag + "}", imageCheckResult.IsMatched.ToString());

                        Console.WriteLine("Image match: " + imageCheckResult.IsMatched.ToString());
                        checker.AddRect(imageCheckResult);
                    }
                    
                    checker.SaveScreen(@"D:\Test\Images\" + DateTime.Now.ToString("HHmmss") + ".png");
                }
                try
                {
                    var answer = new NCalc.Expression(this.Fomula).Evaluate();
                    Console.WriteLine(this.Fomula + " => " + answer);

                    if (answer is bool && (bool)answer)
                    {
                        return true;
                    }
                }
                catch { }
                Thread.Sleep(this.Interval ?? _defaultInterval);
            }

            return false;
        }


    }
}
