using System;
using System.Runtime.InteropServices;

namespace RecordAndPlay
{
    //Start Hooks
    [StructLayout(LayoutKind.Sequential)]
    struct Point
    {
        public int x;
        public int y;
        public override bool Equals(object obj)
        {
            Point otherPoint = (Point)obj;
            return x == otherPoint.x && y == otherPoint.y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MouseHookStruct
    {
        public Point point;
        public int mouseData;
        public int flags;
        public int time;
        public UIntPtr dwExtraInfo;
        public override string ToString()
        {
            return $"(x: {point.x,4},y: {point.y,4})";
        }
    }
    public enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,

        WM_LBUTTONUP = 0x0202,

        WM_MOUSEMOVE = 0x0200,

        WM_MOUSEWHEEL = 0x020A,

        WM_MOUSEHWHEEL = 0x020E,

        WM_RBUTTONDOWN = 0x0204,

        WM_RBUTTONUP = 0x0205
    }

    //END Hooks

    //Start SIMULATE INPUT
    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT
    {
        [FieldOffset(0)]
        public int type;
        [FieldOffset(4)]
        public MOUSEINPUT mouseInput;
        [FieldOffset(4)]
        public KEYBDINPUT keybdInput;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public MouseEventFlags dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }


    [Flags]
    public enum MouseEventFlags : uint
    {
        MOUSEEVENTF_MOVE = 0x0001,
        MOUSEEVENTF_LEFTDOWN = 0x0002,
        MOUSEEVENTF_LEFTUP = 0x0004,
        MOUSEEVENTF_RIGHTDOWN = 0x0008,
        MOUSEEVENTF_RIGHTUP = 0x0010,
        MOUSEEVENTF_MIDDLEDOWN = 0x0020,
        MOUSEEVENTF_MIDDLEUP = 0x0040,
        MOUSEEVENTF_XDOWN = 0x0080,
        MOUSEEVENTF_XUP = 0x0100,
        MOUSEEVENTF_WHEEL = 0x0800,
        MOUSEEVENTF_VIRTUALDESK = 0x4000,
        MOUSEEVENTF_ABSOLUTE = 0x8000
    }
    //END SIMULATE INPUT
}
