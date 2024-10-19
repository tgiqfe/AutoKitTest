using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib.Manifest
{
    internal class TestSceneCluster
    {
        private static readonly string _testSceneDir = Path.Combine(Item.WorkDirectory, "TestScene");

        private List<TestScene> _sceneList = null;

        public bool? TotalResult { get; set; }

        public TestSceneCluster()
        {

        }

        private void LoadSettingFiles()
        {
            this._sceneList = new List<TestScene>();

            if (!Directory.Exists(_testSceneDir)) return;
            string[] extensions = { ".txt", ".yml", ".yaml" };
            var settingFiles = Directory.GetFiles(_testSceneDir).Where(x =>
            {
                string extension = Path.GetExtension(x);
                return extensions.Any(y =>
                    extension.Equals(y, StringComparison.OrdinalIgnoreCase));
            });
            foreach (var file in settingFiles)
            {
                _sceneList.Add(TestScene.Load2(file));
            }
        }

        private void SaveSettingFiles()
        {
            if (!Directory.Exists(_testSceneDir))
            {
                Directory.CreateDirectory(_testSceneDir);
            }
            foreach (var scene in _sceneList)
            {
                scene.Save(Path.Combine(_testSceneDir, scene.Name));
            }
        }

        public void Execute()
        {
            bool interrupt = false;
            foreach (var scene in this._sceneList)
            {
                var ret_command = false;
                foreach (var command in scene.Commands)
                {
                    var testCommand = command.Value;
                    switch (testCommand.Type)
                    {
                        case CommandType.ImageCheck:
                            var imageCheck = new CommandImageCheck(testCommand);
                            ret_command = imageCheck.Execute();
                            break;
                        case CommandType.AppOpen:
                            var appOpen = new CommandAppOpen(testCommand);
                            ret_command = appOpen.Execute();
                            break;
                        case CommandType.AppClose:
                            break;
                        case CommandType.Click:
                            break;
                        default:
                            break;
                    }
                    if (!ret_command && testCommand.Failed == FailedAction.Quit)
                    {
                        interrupt = true;
                        break;
                    }
                }
                if (interrupt) { break; }
            }
        }
    }
}
