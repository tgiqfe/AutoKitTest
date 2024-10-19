using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib.Manifest
{
    internal class TestSceneCluster
    {
        private static readonly string _testFlowsDir = Path.Combine(Item.WorkDirectory, "TestScene");

        private List<TestScene> _sceneList = null;

        public bool? TotalResult { get; set; }

        public TestSceneCluster()
        {
            this._sceneList = new List<TestScene>();
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
