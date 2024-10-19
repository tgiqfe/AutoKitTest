using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace AutoKitTest.Lib.Manifest
{
    internal class TestCommand
    {
        [YamlIgnore]
        public string Name { get; set; }

        [YamlIgnore]
        public CommandType Type { get; set; }

        #region General parameter

        public int? Timeout { get; set; }
        public int? Interval { get; set; }
        public bool? Debug { get; set; }

        [YamlMember(Alias = "Failed")]
        public FailedAction? Failed { get; set; }

        #endregion

        #region for AppOpen

        [YamlMember(Alias = "Exe")]
        public string ApplicationPath { get; set; }

        [YamlMember(Alias = "Args")]
        public string Arguments { get; set; }

        [YamlMember(Alias = "Work")]
        public string WorkingDirectory { get; set; }

        #endregion
        #region for ImageCheck

        public double? Threshould { get; set; }
        public string Fomula { get; set; }
        public List<string> ImageCheck { get; set; }

        #endregion


    }
}
