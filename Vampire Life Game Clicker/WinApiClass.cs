using System;
using System.Runtime.InteropServices;

namespace Vampire_Life_Game_Clicker
{
    public struct MyKeys
    {
        public MyKeys(short scanCode, short wkCode)
        {
            ScanCode = scanCode;
            WkCode = wkCode;
        }
        public short ScanCode;
        public short WkCode;

    }

    public class WinApiClass
    {
        [DllImport("user32.dll")]
        static extern UInt32 SendInput(UInt32 nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] pInputs, Int32 cbSize);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int X, int Y);

        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        const int KEYEVENTF_KEYUP = 0x0002;
        const int KEYEVENTF_UNICODE = 0x0004;
        const int KEYEVENTF_SCANCODE = 0x0008;

        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;

        public void SendKey(MyKeys keys, bool up)
        {
            INPUT[] InputData = new INPUT[1];


            InputData[0].type = 1;
            InputData[0].ki.dwFlags = 0;
            InputData[0].ki.wVk = keys.WkCode;


            if (up)
            {
                InputData[0].ki.dwFlags = 2;

            }

            SendInput(1, InputData, Marshal.SizeOf(typeof(INPUT)));
        }

        public void PressLeftMouse()
        {
            mouse_event((uint)MouseEventFlags.LEFTDOWN, 0, 0, 0, 0);
            mouse_event((uint)MouseEventFlags.LEFTUP, 0, 0, 0, 0);
        }

        public void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }
    }
}