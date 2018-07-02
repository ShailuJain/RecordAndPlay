using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static RecordAndPlay.HookMethods;

namespace RecordAndPlay
{
    class LowLevelMouseListener
    {
        private System.Threading.Timer timer;
        private Dictionary<EventArgs,EventDetails> events;
        public LowLevelMouseListener(Dictionary<EventArgs, EventDetails> events)
        {
            this.events = events;
        }
        private int WH_MOUSE_LL = 14;
        [StructLayout(LayoutKind.Sequential)]
        struct Point
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct MouseHookStruct
        {
            public Point point;
            public IntPtr hwnd;
            public uint wHitTestCode;
            public IntPtr dwExtraInfo;
            public override string ToString()
            {
                return $"(x: {point.x,4},y: {point.y,4})";
            }
        }
        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,

            WM_LBUTTONUP = 0x0202,

            WM_MOUSEMOVE = 0x0200,

            WM_MOUSEWHEEL = 0x020A,

            WM_RBUTTONDOWN = 0x0204,

            WM_RBUTTONUP = 0x0205
        }



        public event EventHandler<MouseEventArgs> MouseClicked;
        public event EventHandler<MouseEventArgs> MouseMoved;

        public HookProc mouseHookProc;
        private IntPtr hookID = IntPtr.Zero;

        public LowLevelMouseListener()
        {
            mouseHookProc = MouseHookProcedure;
            timer = new System.Threading.Timer(TimerCallbackFunc,null,0,1);
        }

        private void TimerCallbackFunc(object state)
        {
            
        }

        public void HookMouse()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                hookID = SetWindowsHookEx(WH_MOUSE_LL, mouseHookProc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public void UnhookMouse()
        {
            UnhookWindowsHookEx(hookID);
        }

        private IntPtr MouseHookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            MouseHookStruct mouseStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
            if (nCode >= 0)
            {
                switch ((MouseMessages)wParam)
                {
                    case MouseMessages.WM_LBUTTONDOWN:
                        //events[new MouseEventArgs(MouseButtons.Left,1,mouseStruct.point.x,mouseStruct.point.y,0)]
                        Console.WriteLine("left down");
                        break;
                    case MouseMessages.WM_LBUTTONUP:
                        Console.WriteLine("left up");
                        break;
                    case MouseMessages.WM_MOUSEMOVE:
                        //Console.WriteLine("mouse move");
                        break;
                    case MouseMessages.WM_MOUSEWHEEL:
                        Console.WriteLine("mouse wheel");
                        break;
                    case MouseMessages.WM_RBUTTONDOWN:
                        Console.WriteLine("right down");
                        break;
                    case MouseMessages.WM_RBUTTONUP:
                        Console.WriteLine("right up");
                        break;
                }
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }
    }

}
