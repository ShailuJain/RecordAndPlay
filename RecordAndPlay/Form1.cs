using System;
using System.Windows.Forms;

namespace RecordAndPlay
{
    public partial class Form1 : Form
    {
        private LowLevelMouseListener lowLevelMouseListener;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            lowLevelMouseListener = new LowLevelMouseListener();
            lowLevelMouseListener.HookMouse();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lowLevelMouseListener.UnhookMouse();
        }
    }
}
