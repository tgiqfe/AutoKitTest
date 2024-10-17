using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace AutoKitTest.Lib.Manifest
{
    internal class TestFlows
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Dictionary<string, TestCommand> Commands { get; set; }

        [YamlIgnore]
        public bool? Result { get; set; }
    }
}
