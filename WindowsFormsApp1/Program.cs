using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            var splashThread = new Thread(() =>
            {
                Application.Run(new Splashform());
            }
            );
            splashThread.Start();
            Thread.Sleep(13000);
            splashThread.Abort();

            var mainThread = new Thread(() =>
            {
                Application.Run(new desktop());
            }
            );
            mainThread.Start();

            /// Run the startup sequence
            ///Application.Run(new Splashform());
            /// System.Threading.Thread.Sleep(3000);
            /// splashform.Close();

            /// Run the main program
            ///Application.Run(new Form2());
        }
    }
}
