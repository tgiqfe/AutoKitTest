using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib
{
    internal enum FailedAction
    {
        None,       //  何もしない。エラーでも次へ進む。
        Quit,       //  エラーが発生したら、その時点でテストを終了する。
    }
}
