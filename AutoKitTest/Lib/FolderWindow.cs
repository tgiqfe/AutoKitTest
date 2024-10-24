using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest.Lib
{
    internal class FolderWindow
    {
        #region for open folder

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern int ShellExecute(IntPtr hwnd, string hWnd, string lpOperation, string lpParameters, string lpDirectory, int nShowCmd);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int SHOpenFolderAndSelectItems(
            IntPtr pidlFolder,
            uint cidl,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] apidl,
            uint dwFlags);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int SHParseDisplayName(
            [MarshalAs(UnmanagedType.LPWStr)] string pszName,
            IntPtr pdc,
            out IntPtr ppidl,
            uint sfgaoIn,
            out uint psfgaoOut);

        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern void CoTaskMemFree(IntPtr pv);

        #endregion
        #region for close folder

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const uint WM_CLOSE = 0x0010;

        #endregion

        /// <summary>
        /// Open folder, or Open folder and select item (sub folder, file)
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="itemName"></param>
        public static void Open(string folderPath, string itemName = null)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                ShellExecute(nint.Zero, "open", folderPath, null, null, 1);
            }
            else
            {
                string item = itemName.Contains("\\") ? Path.GetFileName(itemName) : itemName;
                string itemPath = Path.Combine(folderPath, item);

                uint psfgaoOut;
                nint pidFolder, pidFile;
                var hr_d = SHParseDisplayName(folderPath, nint.Zero, out pidFolder, 0, out psfgaoOut);
                var hr_f = SHParseDisplayName(itemPath, nint.Zero, out pidFile, 0, out psfgaoOut);

                nint[] array = { pidFile };
                var ret = SHOpenFolderAndSelectItems(pidFolder, (uint)array.Length, array, 0);
                CoTaskMemFree(pidFolder);
                CoTaskMemFree(pidFile);
            }
        }

        /// <summary>
        /// Close folde windows
        /// </summary>
        /// <param name="folderPath"></param>
        public static void Close(string folderPath)
        {
            string uri = new Uri(folderPath).ToString().TrimEnd('/');

            var t = Type.GetTypeFromProgID("Shell.Application");
            dynamic o = Activator.CreateInstance(t);
            try
            {
                var ws = o.Windows();
                for (int i = 0; i < ws.Count; i++)
                {
                    var ie = ws.Item(i);
                    if (ie == null) continue;

                    string processName = Path.GetFileName(ie.FullName as string);
                    if (processName.Equals("explorer.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        string locationPath = new Uri(ie.LocationURL as string).ToString().TrimEnd('/');
                        if (locationPath.TrimEnd('/').Equals(uri, StringComparison.OrdinalIgnoreCase))
                        {
                            PostMessage((nint)ie.HWND, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        }
                    }
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(o);
            }
        }
    }
}
