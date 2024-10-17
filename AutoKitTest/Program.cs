using AutoKitTest.Lib;
using AutoKitTest.Lib.Manifest;
using AutoKitTest.Lib.Yaml;
using System.Text.Json;
using YamlDotNet.Serialization;


TestFlows2 flow = new TestFlows2()
{
    Name = "TestFlow",
    Description = "Test Flow Description",
    Commands = new Dictionary<string, TestCommand2>()
    {
        {
            "起動チェック [ImageCheck]",
            new TestCommand2()
            {
                Name = "Command1",
                Threshould = 0.98,
                ImageCheckInterval = 1000,
                ImageCheckTimeout = 10000,
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
    WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
    Build();
serializer.Serialize(Console.Out, flow);


//string yaml = new Serializer().Serialize(flow);
//Console.WriteLine(yaml);


Console.ReadLine();