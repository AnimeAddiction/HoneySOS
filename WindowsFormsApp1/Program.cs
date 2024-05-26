using System;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApp1;

public partial class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Create and initialize the paging form
        paging page = new paging();

        // Show the paging form
        page.Show();

        // Start the message loop on the UI thread
        Application.Run();
    }

    // Method to introduce delay
    static void Delay(int milliseconds)
    {
        Thread.Sleep(milliseconds);
    }
}
