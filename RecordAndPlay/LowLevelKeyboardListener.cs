using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using static RecordAndPlay.NativeMethods;

namespace RecordAndPlay
{
    class LowLevelKeyboardListener
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WH_KEYBOARD_LL = 13;

        public event EventHandler<KeyPressedArgs> OnKeyPressed;

        private HookProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public LowLevelKeyboardListener()
        {
            _proc = HookCallBack;
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
                OnKeyPressed?.Invoke(this, new KeyPressedArgs(KeyInterop.KeyFromVirtualKey(vkCode)));
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }


    public class KeyPressedArgs : EventArgs
    {
        public Key KeyPressed { get; private set; }
        public KeyPressedArgs(Key key)
        {
            this.KeyPressed = key;
        }
    }
}
