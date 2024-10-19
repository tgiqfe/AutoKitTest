using AutoKitTest.Lib;
using AutoKitTest.Lib.Manifest;
using AutoKitTest.Lib.Yaml;
using OpenCvSharp;
using System.Text.Json;
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

var flow = TestScene.Load();

var serializer = new SerializerBuilder().
    WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x)).
    WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
    Build();
serializer.Serialize(Console.Out, flow);

//flow.SetCommandType();
//flow.ExecuteCommand();



/*
Thread.Sleep(3000);
using (ScreenChecker2 checker = new ScreenChecker2())
{
    
    ImageCheckResult result_lt = checker.LocateOnScreen("lt", @"D:\Test\Images\lt.jpg", 0.99);
    checker.AddRect(result_lt);
    ImageCheckResult result_lb = checker.LocateOnScreen("lb", @"D:\Test\Images\lb.jpg", 0.99);
    checker.AddRect(result_lb);
    ImageCheckResult result_rt = checker.LocateOnScreen("rt", @"D:\Test\Images\rt.jpg", 0.99);
    checker.AddRect(result_rt);
    ImageCheckResult result_rb = checker.LocateOnScreen("rb", @"D:\Test\Images\rb.jpg", 0.99);
    checker.AddRect(result_rb);



    checker.SaveScreen(@"D:\Test\Images\210540.png");

    Console.WriteLine("画像一致: " + result_lt.IsMatched.ToString());
    Console.WriteLine("画像一致: " + result_lb.IsMatched.ToString());
    Console.WriteLine("画像一致: " + result_rt.IsMatched.ToString());
    Console.WriteLine("画像一致: " + result_rb.IsMatched.ToString());

}
*/



Console.ReadLine();