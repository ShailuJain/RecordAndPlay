using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Native.NativeMethods;

namespace Events
{
    class LowLevelKeyboardListener
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYUP = 0x0105;

        private const int WH_KEYBOARD_LL = 13;

        private const int KEYEVENTF_KEYUP = 0x0002;
        
        private HookProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private List<KeyValuePair<EventArgs, EventDetails>> events;

        public LowLevelKeyboardListener(List<KeyValuePair<EventArgs, EventDetails>> events)
        {
            _proc = HookCallBack;
            this.events = events;
        }

        public void UnHookKeyboard()
        {
            UnhookWindowsHookEx(_hookID);
        }

        public void HookKeyboard()
        {
            _hookID = SetHook(_proc);
        }

        private IntPtr SetHook(HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                int bScan = Marshal.ReadInt32(lParam, 4);
                events.Add(new KeyValuePair<EventArgs, EventDetails>(new KeyData(vkCode, bScan, 0), new EventDetails(EventType.KEYBOARDEVENT, DelayCounter.Delay)));
                DelayCounter.ResetDelay();
            }
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                int bScan = Marshal.ReadInt32(lParam, 4);
                events.Add(new KeyValuePair<EventArgs, EventDetails>(new KeyData(vkCode, bScan, KEYEVENTF_KEYUP), new EventDetails(EventType.KEYBOARDEVENT, DelayCounter.Delay)));
                DelayCounter.ResetDelay();
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
    public class KeyData : EventArgs
    {
        public int vkCode { get; }
        
        public int scanCode { get; }

        public int dwFlags { get; }

        public KeyData(int vkCode, int scanCode, int dwFlags)
        {
            this.vkCode = vkCode;
            this.scanCode = scanCode;
            this.dwFlags = dwFlags;
        }
    }
}
