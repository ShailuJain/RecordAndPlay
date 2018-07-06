using Events;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace UI
{
    public partial class Form1 : Form
    {
        private LowLevelMouseListener lowLevelMouseListener;
        private LowLevelKeyboardListener lowLevelKeyboardListener;
        private EventPlayer eventPlayer;
        private List<KeyValuePair<EventArgs, EventDetails>> events;
        public Form1()
        {
            InitializeComponent();
            events = new List<KeyValuePair<EventArgs, EventDetails>>();
            lowLevelMouseListener = new LowLevelMouseListener(events);
            lowLevelKeyboardListener = new LowLevelKeyboardListener(events);
            eventPlayer = new EventPlayer();
            btnPlay.Enabled = false;
            btnStop.Enabled = false;
        }


        private void btnRecord_Click(object sender, EventArgs e)
        {
            events.Clear();
            lowLevelMouseListener.HookMouse();
            lowLevelKeyboardListener.HookKeyboard();
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lowLevelMouseListener.UnhookMouse();
            lowLevelKeyboardListener.UnHookKeyboard();
            btnPlay.Enabled = true;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            eventPlayer.Play(events);
        }
    }
}
