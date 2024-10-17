using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace AutoKitTest.Lib.Manifest
{
    internal class TestCommand2
    {
        [YamlIgnore]
        public string Name { get; set; }

        public double? Threshould { get; set; }
        public int? ImageCheckInterval { get; set; }
        public int? ImageCheckTimeout { get; set; }
        public string Fomula { get; set; }
        public List<string> ImageCheck { get; set; }

        public string SampleProperty { get; set; }
    }
}
