using AutoKitTest.Lib.Yaml;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace AutoKitTest.Lib.Manifest
{
    internal class TestScene
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, TestCommand> Commands { get; set; }

        [YamlIgnore]
        public bool? Result { get; set; }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="settingFile"></param>
        /// <returns></returns>
        public static TestScene Load(string settingFile)
        {
            var yaml = File.ReadAllText(settingFile);
            var testScene = new Deserializer().Deserialize<TestScene>(yaml);

            Regex typePattern = new Regex(@"\[[^\[\]]+\]$");
            foreach (var command in testScene.Commands)
            {
                if (typePattern.IsMatch(command.Key))
                {
                    command.Value.Name = typePattern.Replace(command.Key, "").Trim();
                    command.Value.Type = (CommandType)Enum.Parse(typeof(CommandType), typePattern.Match(command.Key).Value.Trim('[', ']'));
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
        /// <param name="settingFile"></param>
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
