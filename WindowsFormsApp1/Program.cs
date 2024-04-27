using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


/// this
/// ghg

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /// Run the startup sequence
            var splashform = new Splashform();
            splashform.Show();
            System.Threading.Thread.Sleep(3000);

            /// Run the main program
            Application.Run(new Form1());
        }
    }
}
