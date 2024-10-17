using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib.Manifest
{
    internal class TestFlows
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Dictionary<string, TestCommand> Commands { get; set; }
    }
}
