

using AutoKitTest.Lib;

var list = new List<ImageItem>() {
    new()
    {
        Path = @"E:\Test\Images\001.jpg",
        Threshold = 0.95,
    },
    new()
    {
        Path = @"E:\Test\Images\002.png",
        Threshold = 0.95,
    },
    new()
    {
        Path = @"E:\Test\Images\003.jpg",
        Threshold = 0.95,
    }
};

using (var checker = new ScreenChecker(list, true))
{
    checker.LocateOnScreen();
}

Console.ReadLine();