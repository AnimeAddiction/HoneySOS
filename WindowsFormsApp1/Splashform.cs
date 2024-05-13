using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Splashform : Form
    {

        private Timer timer;
        public Splashform()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            InitializeTimer();

        }
        private void InitializeTimer()
        {
            // Create and configure the timer
            timer = new Timer();
            timer.Interval = 13000; // Set the duration in milliseconds (9000 ms = 9 seconds)
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Close the splash screen when the timer ticks
            Close();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}
