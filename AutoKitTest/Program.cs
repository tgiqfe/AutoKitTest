
using AutoKitTest.Lib.Manifest;
using Microsoft.VisualBasic;

TestSceneCluster cluster = new();
//cluster.LoadSettingFiles();
cluster.List = new List<TestScene>()
{
    new()
    {
        Name = "Google Chrome",
        Description = "Test Google Chrome",
        Commands = new()
        {
            {
                "Image check (start wait) [ImageCheck]",
                new()
                {
                    Timeout = 10000,
                    Interval = 5000,
                    Fomula = "{001}",
                    ImageCheck = new()
                    {
                        @"001, D:\Test\Images\aaaa.jpg",
                    }
                }
            }
        }
    }
};




Console.ReadLine();
