using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib.Manifest
{
    internal class CommandWait
    {
        public bool Enabled { get; set; }

        #region from TestCommand parameters

        public int Timeout { get; set; }
        public FailedAction FailedAction { get; set; }
        
        #endregion

        const int _defaultTimeout = 5000;
        private static readonly FailedAction _defaultFailedAction = FailedAction.None;

        public CommandWait(TestCommand command)
        {
            this.Timeout = command.Timeout ?? _defaultTimeout;
            this.FailedAction = command.Failed ?? _defaultFailedAction;

            ParameterCheck();
        }

        private void ParameterCheck()
        {
            var ret = true;
            ret &= this.Timeout > 1000;

            this.Enabled = ret;
        }

        public bool Execute()
        {
            if (!this.Enabled) return false;

            Thread.Sleep(this.Timeout);

            return true;
        }
    }
}
