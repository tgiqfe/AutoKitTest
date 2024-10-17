using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private Regex _typePattern = new Regex(@"\[[^\[\]]+\]$");

        public void SetCommandType()
        {
            foreach (var command in this.Commands)
            {
                if (_typePattern.IsMatch(command.Key))
                {
                    command.Value.Name = _typePattern.Replace(command.Key, "").Trim();
                    command.Value.Type = (Commands)Enum.Parse(typeof(Commands), _typePattern.Match(command.Key).Value.Trim('[', ']'));
                }
                else
                {
                    command.Value.Name = command.Key;
                    command.Value.Type = Manifest.Commands.None;
                }
            }
        }
    }
}
