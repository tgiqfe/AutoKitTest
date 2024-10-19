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

        /// <summary>
        /// Load
        /// </summary>
        /// <returns></returns>
        public static List<TestScene> Load(string targetDir)
        {
            List<TestScene> list = new();

            if (!Directory.Exists(targetDir)) return list;
            string[] extensions = { ".txt", ".yml", ".yaml" };
            var settingFiles = Directory.GetFiles(targetDir).Where(x =>
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

        public static TestScene Load2(string settingFile)
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

        /// <summary>
        /// Save
        /// </summary>
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
                    command.Value.Type = (CommandType)Enum.Parse(typeof(CommandType), _typePattern.Match(command.Key).Value.Trim('[', ']'));
                }
                else
                {
                    command.Value.Name = command.Key;
                    command.Value.Type = Manifest.CommandType.None;
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
                    case Manifest.CommandType.ImageCheck:
                        var imagecheck = new CommandImageCheck(command.Value);
                        this.Result = imagecheck.Execute();
                        Console.WriteLine(this.Result);
                        break;
                    case Manifest.CommandType.AppOpen:
                        break;
                    case Manifest.CommandType.AppClose:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
