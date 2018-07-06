using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static RecordAndPlay.NativeMethods;

namespace RecordAndPlay
{
    class LowLevelMouseListener
    {
        private int WH_MOUSE_LL = 14;

        public HookProc mouseHookProc;
        private IntPtr hookID = IntPtr.Zero;
        private List<KeyValuePair<EventArgs, EventDetails>> events;

        public LowLevelMouseListener(List<KeyValuePair<EventArgs, EventDetails>> events)
        {
            mouseHookProc = MouseHookProcedure;
            this.events = events;
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
                        events.Add(new KeyValuePair<EventArgs, EventDetails>(new MouseData(mouseStruct.point.x, mouseStruct.point.y, 0, (MouseEventFlags.MOUSEEVENTF_LEFTDOWN), (uint)mouseStruct.time, mouseStruct.dwExtraInfo), new EventDetails(EventType.MOUSEEVENT, DelayCounter.Delay)));
                        DelayCounter.ResetDelay();
                        break;
                    case MouseMessages.WM_LBUTTONUP:
                        events.Add(new KeyValuePair<EventArgs, EventDetails>(new MouseData(mouseStruct.point.x, mouseStruct.point.y, 0, (MouseEventFlags.MOUSEEVENTF_LEFTUP), (uint)mouseStruct.time, mouseStruct.dwExtraInfo), new EventDetails(EventType.MOUSEEVENT, DelayCounter.Delay)));
                        DelayCounter.ResetDelay();
                        break;
                    case MouseMessages.WM_MOUSEMOVE:
                        events.Add(new KeyValuePair<EventArgs, EventDetails>(new MouseData(mouseStruct.point.x, mouseStruct.point.y, 0, (MouseEventFlags.MOUSEEVENTF_ABSOLUTE | MouseEventFlags.MOUSEEVENTF_MOVE), (uint)mouseStruct.time, mouseStruct.dwExtraInfo), new EventDetails(EventType.MOUSEEVENT, DelayCounter.Delay)));
                        DelayCounter.ResetDelay();
                        break;
                    case MouseMessages.WM_MOUSEWHEEL:
                        int mouseData = mouseStruct.mouseData;
                        events.Add(new KeyValuePair<EventArgs, EventDetails>(new MouseData(mouseStruct.point.x, mouseStruct.point.y, mouseData, (MouseEventFlags.MOUSEEVENTF_WHEEL), (uint)mouseStruct.time, mouseStruct.dwExtraInfo), new EventDetails(EventType.MOUSEEVENT, DelayCounter.Delay)));
                        DelayCounter.ResetDelay();
                        break;
                    case MouseMessages.WM_RBUTTONDOWN:
                        events.Add(new KeyValuePair<EventArgs, EventDetails>(new MouseData(mouseStruct.point.x, mouseStruct.point.y, 0, (MouseEventFlags.MOUSEEVENTF_RIGHTDOWN), (uint)mouseStruct.time, mouseStruct.dwExtraInfo), new EventDetails(EventType.MOUSEEVENT, DelayCounter.Delay)));
                        DelayCounter.ResetDelay();
                        break;
                    case MouseMessages.WM_RBUTTONUP:
                        events.Add(new KeyValuePair<EventArgs, EventDetails>(new MouseData(mouseStruct.point.x, mouseStruct.point.y, 0, (MouseEventFlags.MOUSEEVENTF_RIGHTUP), (uint)mouseStruct.time, mouseStruct.dwExtraInfo),new EventDetails(EventType.MOUSEEVENT, DelayCounter.Delay)));
                        DelayCounter.ResetDelay();
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
        public int mouseData;
        public MouseEventFlags dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;

        public MouseData(int dx, int dy, int mouseData, MouseEventFlags dwFlags, uint time, UIntPtr dwExtraInfo)
        {
            this.dx = dx;
            this.dy = dy;
            this.mouseData = mouseData;
            this.dwFlags = dwFlags;
            this.time = time;
            this.dwExtraInfo = dwExtraInfo;
        }
    }
}
