using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace RecordAndPlay
{
    public partial class Form1 : Form
    {
        private LowLevelMouseListener lowLevelMouseListener;
        private EventPlayer eventPlayer;
        private Dictionary<EventArgs, EventDetails> dictionary;
        public Form1()
        {
            InitializeComponent();
            dictionary = new Dictionary<EventArgs, EventDetails>();
            lowLevelMouseListener = new LowLevelMouseListener();
            eventPlayer = new EventPlayer();
            lowLevelMouseListener.MouseMove += MouseMove;
        }

        private new void MouseMove(object sender, MouseData e)
        {
            dictionary[e] = e.eventDetails;
            textBox1.Text += "X : " + e.dx + " Y : " + e.dy;
        }


        private void btnRecord_Click(object sender, EventArgs e)
        {
            lowLevelMouseListener.HookMouse();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lowLevelMouseListener.UnhookMouse();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button Play clicked");
            //eventPlayer.play(dictionary);
            int w = 65535 / Screen.PrimaryScreen.Bounds.Width;
            int h = 65535 / Screen.PrimaryScreen.Bounds.Height;
            INPUT[] input = new INPUT[] {
                new INPUT {
                    type = 0,
                    mouseInput = new MOUSEINPUT {
                        dx = 100,
                        dy = 100,
                        dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN 
                        | MouseEventFlags.MOUSEEVENTF_ABSOLUTE 
                    }
                },
                new INPUT {
                    type = 0,
                    mouseInput = new MOUSEINPUT {
                        dx = 100,
                        dy = 100,
                        dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP
                        | MouseEventFlags.MOUSEEVENTF_ABSOLUTE
                    }
                }
            };
            Console.WriteLine(NativeMethods.SendInput((UInt32)input.Length, input, Marshal.SizeOf(typeof(INPUT))));
            Thread.Sleep(500);
            
            //Console.WriteLine(Marshal.GetLastWin32Error());
        }
    }
}
