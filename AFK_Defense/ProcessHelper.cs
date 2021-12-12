using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AFK_Defense
{
    public static class ProcessHelper
    {
        #region DLL Imports

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, EnumForWindow enumVal);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool PostMessage(int hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern bool IsZoomed(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, ref nint lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, nint lParam);

        #endregion

        private enum EnumForWindow
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };

        public static void SendKeyToProcess(string processName)
        {
            const int WM_SYSKEYDOWN = 0x0104;
            const int BCM_SETNOTE = 0xffff;
            const int VK_W = 0x57;

            Console.WriteLine("writing to notepad");

            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process == null) return;

            SendMessage(process.MainWindowHandle, BCM_SETNOTE, 0, "My CommandLink Note");

            //PostMessage(process.MainWindowHandle.ToInt32(), WM_SYSKEYDOWN, VK_W, 0);
        }

        public static void SetForeGroundMainWindowHandle(string processName)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process == null) return;

            var isMinimized = IsIconic(process.MainWindowHandle);
            var isMaximized = IsZoomed(process.MainWindowHandle);

            var displayRequest = EnumForWindow.Show;
            if (isMinimized) displayRequest = EnumForWindow.Restore;
            if (isMaximized) displayRequest = EnumForWindow.Show;

            ShowWindow(process.MainWindowHandle, displayRequest);
            System.Threading.Thread.Sleep(3000);
            _ = SetForegroundWindow(process.MainWindowHandle);
        }

        public static bool CheckIfProcessIsRunning(string processName)
        {
            return Process.GetProcessesByName(processName).Any();
        }
    }
}
