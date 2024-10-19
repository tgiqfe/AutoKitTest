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
        private static readonly string _testFlowsDir = Path.Combine(Item.WorkDir, "TestScene");
        private Regex _typePattern = new Regex(@"\[[^\[\]]+\]$");

        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, TestCommand> Commands { get; set; }

        [YamlIgnore]
        public bool? Result { get; set; }

        /// <summary>
        /// Load
        /// </summary>
        /// <returns></returns>
        public static List<TestScene> Load()
        {
            List<TestScene> list = new();

            if (!Directory.Exists(_testFlowsDir)) return list;
            string[] extensions = { ".txt", ".yml", ".yaml" };
            var settingFiles = Directory.GetFiles(_testFlowsDir).Where(x =>
            {
                string extension = Path.GetExtension(x);
                return extensions.Any(y =>
                    extension.Equals(y, StringComparison.OrdinalIgnoreCase));
            });
            foreach (var file in settingFiles)
            {
                var yaml = File.ReadAllText(file);
                var testScene = new Deserializer().Deserialize<TestScene>(yaml);
                testScene.SetCommandType();
                list.Add(testScene);
            }
            return list;
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save()
        {
            var serializer = new SerializerBuilder().
                WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x)).
                WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
                Build();
            if(!Directory.Exists(_testFlowsDir))
            {
                Directory.CreateDirectory(_testFlowsDir);
            }
            using (var writer = new StreamWriter(Path.Combine(_testFlowsDir, this.Name), false, Encoding.UTF8))
            {
                serializer.Serialize(writer, this);
            }
        }

        /// <summary>
        /// Load時にCommand部分を、Keyの記載から判定してTypeをセット
        /// </summary>
        private void SetCommandType()
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

        /// <summary>
        /// Command部分を実行
        /// </summary>
        public void ExecuteCommand()
        {
            foreach (var command in this.Commands)
            {
                switch (command.Value.Type)
                {
                    case Manifest.Commands.ImageCheck:
                        var imagecheck = new CommandImageCheck(command.Value);
                        this.Result = imagecheck.Execute();
                        Console.WriteLine(this.Result);
                        break;
                    case Manifest.Commands.AppOpen:
                        break;
                    case Manifest.Commands.AppClose:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
