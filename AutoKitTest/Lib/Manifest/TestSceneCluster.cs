using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib.Manifest
{
    internal class TestSceneCluster
    {
        private static readonly string _testFlowsDir = Path.Combine(Item.WorkDir, "TestScene");

        private List<TestScene> _sceneList = null;

        private TestScene _testFlows = null;

        public TestSceneCluster(TestScene testFlows)
        {
            _testFlows = testFlows;
        }

        public void Execute()
        {
            foreach (var command in _testFlows.Commands)
            {
                var testCommand = command.Value;
                switch (testCommand.Type)
                {
                    case Commands.ImageCheck:
                        var imageCheck = new CommandImageCheck(command.Value);
                        imageCheck.Execute();
                        break;
                    case Commands.AppOpen:
                        break;
                    case Commands.AppClose:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
