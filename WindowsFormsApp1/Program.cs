using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show the splash screen on the main UI thread
            Splashform splashForm = new Splashform();
            Application.Run(splashForm);

            // After the splash screen is closed, start the main form on the main UI thread
            //Application.Run(new desktop());

            // This is a test for the voice thing
            Application.Run(new Form1());
        }
    }
}
