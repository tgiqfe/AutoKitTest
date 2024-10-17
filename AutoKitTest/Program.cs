using AutoKitTest.Lib;
using AutoKitTest.Lib.Manifest;
using AutoKitTest.Lib.Yaml;
using System.Text.Json;
using YamlDotNet.Serialization;


TestFlows flow = new TestFlows()
{
    Name = "Adobe Reader",
    Description = "Test Adobe Reader.",
    Commands = new Dictionary<string, TestCommand>()
    {
        {
            "起動チェック[ImageCheck]",
            new TestCommand()
            {
                Name = "Command1",
                Threshould = 0.98,
                Interval = 1000,
                Timeout = 10000,
                Fomula = "{lt} && {lb} && {rt} && {rb}",
                ImageCheck = new List<string>()
                {
                    @"lt, D:\Test\template\images\lt01.jpg, 0.99",
                    @"lb, D:\Test\template\images\lt02.jpg, 0.95",
                    @"rt, D:\Test\template\images\lt03.jpg",
                    @"rb, D:\Test\template\images\lt04.jpg",
                    @"test, ""C:\aaa, 0.55"", 0.98"
                }
            }
        }
    }
};


var serializer = new SerializerBuilder().
    WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x)).
    WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
    Build();
serializer.Serialize(Console.Out, flow);




Console.ReadLine();