using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.Diagnostics;

namespace AFK_Defense
{
    public static class ProcessHelper
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, EnumForWindow enumVal);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        private enum EnumForWindow
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };

        public static void SetForeGroundMainWindowHandle(string processName)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process == null) return;

            ShowWindow(process.MainWindowHandle, EnumForWindow.Restore);
            System.Threading.Thread.Sleep(3000);
            _ = SetForegroundWindow(process.MainWindowHandle);
        }

        public static bool CheckIfProcessIsRunning(string processName)
        {
            Console.WriteLine($"Checking if {processName} is actively running.");
            return Process.GetProcessesByName(processName).Any();
        }
    }
}
