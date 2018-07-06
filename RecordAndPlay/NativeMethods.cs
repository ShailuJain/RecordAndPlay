using System;
using System.Runtime.InteropServices;

namespace RecordAndPlay
{
    class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        //[DllImport("user32.dll",SetLastError = true)]
        //public static extern UInt32 SendInput(int nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData,int dwExtraInfo);

        //[DllImport("user32.dll", SetLastError = false)]
        //public static extern IntPtr GetMessageExtraInfo();o

        [DllImport("user32.dll")]
        public static extern void keybd_event(int bVk, int bScan, uint dwFlags, UIntPtr dwExtraInfo);

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}
