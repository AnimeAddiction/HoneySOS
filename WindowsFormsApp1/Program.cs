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
            MemoryManager mm = new MemoryManager(64, 4); // Total memory: 64, Page size: 4

            mm.AllocateMemory(1, 10);
            mm.VisualizeMemory();

            mm.AllocateMemory(2, 20);
            mm.VisualizeMemory();

            mm.DeallocateMemory(1);
            mm.VisualizeMemory();

            mm.AllocateMemory(3, 15);
            mm.VisualizeMemory();

            mm.DeallocateMemory(2);
            mm.VisualizeMemory();

            // Show the splash screen on the main UI thread
            /*  Splashform splashForm = new Splashform();
              Application.Run(splashForm);

              // After the splash screen is closed, start the main form on the main UI thread
              Application.Run(new desktop()); */
        }
    }
}
