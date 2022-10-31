using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace LeapMotionCursorWithPoints
{
    class MouseCursor
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;
        private const UInt32 MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const UInt32 MOUSEEVENTF_RIGHTUP = 0x10;
        // private const UInt32 MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        // private const UInt32 MOUSEEVENTF_MIDDLEUP = 0x0040;

        public static void MoveCursor(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void SendUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }

        public static void SendDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
        }

        public static void SendUpRight()
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, new System.IntPtr());
        }

        public static void SendDownRight()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, new System.IntPtr());
        }

        public static void SendClick()
        {
            SendDown();
            SendUp();
        }

        public static void SendRightClick()
        {
            SendDownRight();
            SendUpRight();
        }
    }
}
