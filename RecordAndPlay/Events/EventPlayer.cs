using Events;
using Native;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Events
{
    class EventPlayer
    {
        
        public void Play(List<KeyValuePair<EventArgs, EventDetails>> events)
        {
            foreach(var item in events){
                EventDetails eventDetails = item.Value;
                if(eventDetails.eventType == EventType.MOUSEEVENT)
                {
                    int delay = eventDetails.delay + 10; ;
                    MouseData mouseData = item.Key as MouseData;
                    if (mouseData.dwFlags == MouseEventFlags.MOUSEEVENTF_LEFTUP || mouseData.dwFlags == MouseEventFlags.MOUSEEVENTF_WHEEL)
                    {
                        delay = eventDetails.delay + 1000;
                    }
                    int w = 65535 / Screen.PrimaryScreen.Bounds.Width;
                    int h = 65535 / Screen.PrimaryScreen.Bounds.Height;
                    int x = mouseData.dx * w;
                    int y = mouseData.dy * h + 500;
                    Thread.Sleep(delay);
                    NativeMethods.mouse_event((int)mouseData.dwFlags,x, y, mouseData.mouseData, (int)mouseData.dwExtraInfo);
                }
                else if(eventDetails.eventType == EventType.KEYBOARDEVENT)
                {
                    Console.WriteLine("key pressed");
                    KeyData keyEvent = item.Key as KeyData;
                    Thread.Sleep(eventDetails.delay + 300);
                    NativeMethods.keybd_event(keyEvent.vkCode, keyEvent.scanCode, (uint)keyEvent.dwFlags, UIntPtr.Zero);
                }
            }
        }
    }
}