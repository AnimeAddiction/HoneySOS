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

        // Initialize the MemoryManager with the paging form
        MemoryManager mm = new MemoryManager(64, 4, page);

        // Perform memory management operations as needed
        mm.AllocateMemory(1, 10);
        mm.VisualizeMemory();
        Delay(2000); // Delay for 2 seconds

        mm.AllocateMemory(2, 20);
        mm.VisualizeMemory();
        Delay(2000); // Delay for 2 seconds

        mm.DeallocateMemory(1);
        mm.VisualizeMemory();
        Delay(2000); // Delay for 2 seconds

        mm.AllocateMemory(3, 15);
        mm.VisualizeMemory();
        Delay(2000); // Delay for 2 seconds

        mm.DeallocateMemory(2);
        mm.VisualizeMemory();
        Delay(2000); // Delay for 2 seconds

        // Start the message loop on the UI thread
        Application.Run();
    }

    // Method to introduce delay
    static void Delay(int milliseconds)
    {
        Thread.Sleep(milliseconds);
    }
}
