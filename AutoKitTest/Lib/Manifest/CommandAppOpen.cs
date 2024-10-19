using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoKitTest.Lib.Manifest
{
    internal class CommandAppOpen
    {
        #region from TestCommand parameters

        //  General parameter
        public string Name { get; set; }
        public FailedAction FailedAction { get; set; } = FailedAction.Quit;

        //  for AppOpen paraemter
        public string ApplicationPath { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }

        #endregion

        public bool Enabled { get; set; }

        private static Regex _fullpathPattern = new Regex(@"^([a-zA-Z]:\\)|(\\\\)");
        private static readonly FailedAction _defaultFailedAction = FailedAction.Quit;
        private static readonly string _defaultWorkingDirectory = Item.WorkDirectory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="command"></param>
        public CommandAppOpen(TestCommand command)
        {
            this.Name = command.Name;
            this.FailedAction = command.Failed ?? _defaultFailedAction;
            this.ApplicationPath = command.ApplicationPath;
            this.Arguments = command.Arguments;
            this.WorkingDirectory = command.WorkingDirectory ?? _defaultWorkingDirectory;

            if (!_fullpathPattern.IsMatch(ApplicationPath))
            {
                ApplicationPath = Path.Combine(Item.WorkDirectory, ApplicationPath);
            }
            if (!_fullpathPattern.IsMatch(WorkingDirectory))
            {
                WorkingDirectory = Path.Combine(Item.WorkDirectory, WorkingDirectory);
            }

            ParameterCheck();
        }

        private void ParameterCheck()
        {
            var ret = true;
            ret &= (!string.IsNullOrEmpty(this.ApplicationPath) && File.Exists(this.ApplicationPath));

            this.Enabled = ret;
        }

        public bool Execute()
        {
            using (var proc = new Process())
            {
                proc.StartInfo.FileName = this.ApplicationPath;
                proc.StartInfo.Arguments = this.Arguments;
                proc.StartInfo.WorkingDirectory = this.WorkingDirectory;
                proc.StartInfo.CreateNoWindow = false;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
            }

            return true;
        }
    }
}
