using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace RecordAndPlay
{
    class EventPlayer
    {
        public void play(Dictionary<EventArgs, EventDetails> events)
        {
            foreach(var item in events){
                EventDetails eventDetails = item.Value;
                if(eventDetails.eventType == EventType.MOUSEEVENT)
                {
                    MouseData mouseData = item.Key as MouseData;
                    INPUT[] inputs = new INPUT[]
                    {
                        new INPUT()
                        {
                            type = 0,
                            mouseInput = new MOUSEINPUT
                            {
                                dx = mouseData.dx,
                                dy = mouseData.dy,
                                mouseData = mouseData.mouseData,
                                dwFlags = mouseData.dwFlags,
                                time = mouseData.time,
                                dwExtraInfo = mouseData.dwExtraInfo
                            }
                        },
                    };
                    Thread.Sleep((int)eventDetails.delay);
                    Form1.textBox2.Text += "X : " + mouseData.dx + " Y : " + mouseData.dy;
                    uint res = NativeMethods.SendInput((uint)inputs.Length,inputs, Marshal.SizeOf(typeof(INPUT)));
                    if (res != inputs.Length)
                        throw new Exception("Some simulated input commands were not sent successfully. The most common reason for this happening are the security features of Windows including User Interface Privacy Isolation (UIPI). Your application can only send commands to applications of the same or lower elevation. Similarly certain commands are restricted to Accessibility/UIAutomation applications. Refer to the project home page and the code samples for more information.");
                    //Console.WriteLine(res);
                }    
            
            }
        }
    }
}