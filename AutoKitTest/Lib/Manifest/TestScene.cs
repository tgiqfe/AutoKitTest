using AutoKitTest.Lib.Yaml;
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
    internal class TestScene
    {
        private static Regex _typePattern = new Regex(@"\[[^\[\]]+\]$");

        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, TestCommand> Commands { get; set; }

        [YamlIgnore]
        public bool? Result { get; set; }

        public static TestScene Load(string settingFile)
        {
            var yaml = File.ReadAllText(settingFile);
            var testScene = new Deserializer().Deserialize<TestScene>(yaml);

            foreach (var command in testScene.Commands)
            {
                if (_typePattern.IsMatch(command.Key))
                {
                    command.Value.Name = _typePattern.Replace(command.Key, "").Trim();
                    command.Value.Type = (CommandType)Enum.Parse(typeof(CommandType), _typePattern.Match(command.Key).Value.Trim('[', ']'));
                }
                else
                {
                    command.Value.Name = command.Key;
                    command.Value.Type = Manifest.CommandType.None;
                }
            }

            return testScene;
        }

        public void Save(string settingFile)
        {
            var serializer = new SerializerBuilder().
                WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x)).
                WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
                Build();
            using (var writer = new StreamWriter(settingFile, false, Encoding.UTF8))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}
