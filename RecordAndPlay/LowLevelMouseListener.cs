using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static RecordAndPlay.NativeMethods;

namespace RecordAndPlay
{
    class LowLevelMouseListener
    {
        /// <summary>
        /// event generators
        /// </summary>
        public event EventHandler<MouseData> MouseClick;
        public event EventHandler<MouseData> MouseMove;
        public event EventHandler<MouseData> MouseDoubleClick;
        public event EventHandler<MouseData> MouseDown;
        public event EventHandler<MouseData> MouseUp;

        private long delay = 0;
        private Point prePoint;
        private bool LButtonDown = false;
        private System.Threading.Timer timer;
        private Dictionary<EventArgs,EventDetails> events;
        private int WH_MOUSE_LL = 14;

        

        public HookProc mouseHookProc;
        private IntPtr hookID = IntPtr.Zero;

        public LowLevelMouseListener()
        {
            mouseHookProc = MouseHookProcedure;
        }

        private void TimerCallbackFunc(object state)
        {
            delay++;
        }

        public void HookMouse()
        {

            timer = new System.Threading.Timer(TimerCallbackFunc, null, 0, 1);
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
                        LButtonDown = true;
                        prePoint = mouseStruct.point;
                        Console.WriteLine("left down");
                        break;
                    case MouseMessages.WM_LBUTTONUP:
                        if (LButtonDown && mouseStruct.point.Equals(prePoint))
                        {
                            //events[new MouseEventArgs(MouseButtons.Left, 1, mouseStruct.point.x, mouseStruct.point.y, 0)] = new EventDetails(EventType.MOUSEEVENT, delay);
                            delay = 0;
                        }
                        Console.WriteLine("left up");
                        break;
                    case MouseMessages.WM_MOUSEMOVE:
                        MouseMove?.Invoke(this, new MouseData(mouseStruct.point.x, mouseStruct.point.y, 0, (MouseEventFlags.MOUSEEVENTF_ABSOLUTE | MouseEventFlags.MOUSEEVENTF_MOVE), (uint)mouseStruct.time, mouseStruct.dwExtraInfo, new EventDetails(EventType.MOUSEEVENT, delay)));
                        delay = 0;
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

    public class MouseData : EventArgs
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public MouseEventFlags dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
        public EventDetails eventDetails;

        public MouseData(int dx, int dy, uint mouseData, MouseEventFlags dwFlags, uint time, UIntPtr dwExtraInfo, EventDetails eventDetails)
        {
            this.dx = dx;
            this.dy = dy;
            this.mouseData = mouseData;
            this.dwFlags = dwFlags;
            this.time = time;
            this.dwExtraInfo = dwExtraInfo;
            this.eventDetails = eventDetails;
        }
    }
}
