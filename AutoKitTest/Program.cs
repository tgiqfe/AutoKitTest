using AutoKitTest.Lib.Manifest;
using AutoKitTest.Lib.Yaml;
using System.Diagnostics;
using YamlDotNet.Serialization;


/*
TestScene flow = new TestScene()
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
                    @"lt, D:\Test\Images\lt.jpg, 0.99",
                    @"lb, D:\Test\Images\lb.jpg, 0.95",
                    @"rt, D:\Test\Images\rt.jpg",
                    @"rb, D:\Test\Images\rb.jpg"
                }
            }
        }
    }
};
*/

/*
TestScene flow = new TestScene()
{
    Name = "Adobe Reader",
    Description = "Test Adobe Reader.",
    Commands = new Dictionary<string, TestCommand>()
    {
        {
            "起動[AppOpen]",
            new TestCommand()
            {
                Name = "Open",
                ApplicationPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                Arguments = "https://www.yahoo.co.jp"
            }
        }
    }
};
*/

TestScene scenes = new TestScene()
{
    Name = "Adobe Reader",
    Description = "Test Adobe Reader.",
    Commands = new Dictionary<string, TestCommand>()
    {
        {
            "5秒待機[Wait]",
            new TestCommand()
            {
                Timeout = 5000,
            }
        }
    }
};


//var scenes = new TestSceneCluster();
//scenes.LoadSettingFiles();

var serializer = new SerializerBuilder().
    WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x)).
    WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
    Build();
serializer.Serialize(Console.Out, scenes);


scenes.Execute();



Console.ReadLine();