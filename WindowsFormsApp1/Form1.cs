using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private System.Threading.Timer timer; // Timer for periodic updates
        private int processID = 0;
        private DateTime initialTime = DateTime.Now;
        private int schedulingPolicyNum = 0;
        private int currentRowIndex = 0;
        private int previousRowIndex = 0;
        private int timeQuantum = 2;
        private MemoryManager mem;
        private bool touched = false;


        public DataGridView dataGridView4;
        public HoneyOS page;
        private List<int> allocatedProcesses = new List<int>();

        private int totalMemoryUsed = 0;

        public Form1(DataGridView datagridview, MemoryManager memoryManager)
        {
            InitializeTimer(); // Initialize the timer
            dataGridView4 = datagridview;
            mem = memoryManager;

        }

        private void label1_Click(object sender, EventArgs e) { }

        private void InitializeTimer()
        {
            // Create a timer that calls TimerCallback every 1000 milliseconds
            timer = new System.Threading.Timer(TimerCallback, null, 0, 1000);
        }

        // Method to add a row to the DataGridView
        public void AddProcess()
        {
            if (dataGridView4.InvokeRequired)
            {
                // If called from a non-UI thread, invoke on the UI thread
                dataGridView4.Invoke(new Action(AddProcess));
            }
            else
            {
                // Add a new row to the DataGridView
                DataGridViewRow row = new DataGridViewRow();

                DataGridViewCell cell1 = new DataGridViewTextBoxCell();
                cell1.Value = ProcessIdGenerator();
                row.Cells.Add(cell1);

                DataGridViewCell cell2 = new DataGridViewTextBoxCell();
                cell2.Value = BurstTimeGenerator();
                row.Cells.Add(cell2);

                DataGridViewCell cell3 = new DataGridViewTextBoxCell();
                cell3.Value = MemorySizeGenerator();
                row.Cells.Add(cell3);

                DataGridViewCell cell4 = new DataGridViewTextBoxCell();
                cell4.Value = ArrivalTimeGenerator();
                row.Cells.Add(cell4);

                DataGridViewCell cell5 = new DataGridViewTextBoxCell();
                cell5.Value = PriorityGenerator();
                row.Cells.Add(cell5);

                DataGridViewCell cell6 = new DataGridViewTextBoxCell();

                //String status = StatusGenerator(row);
                if (AllocateMemory(int.Parse(cell1.Value.ToString()),int.Parse(cell3.Value.ToString()))){
                    cell6.Value = "Ready Queue";
                }
                else
                {
                    cell6.Value = "Job Queue";
                }
                row.Cells.Add(cell6);
                dataGridView4.Rows.Add(row);

                /**if (status == "Ready")
                {
                    int memorySize = int.Parse(cell3.Value.ToString());
                    if (!allocatedProcesses.Contains(int.Parse(cell1.Value.ToString())) && CheckAvailableMemory(memorySize))
                    {
                        // Allocate memory for the process
                        AllocateMemory(memorySize);
                        allocatedProcesses.Add(int.Parse(cell1.Value.ToString()));
                        Console.WriteLine("Memory Used = " + totalMemoryUsed);
                    }
                }**/
            }
        }

        /**public void AllocateMemory(int memory)
        {
            totalMemoryUsed += memory;
        }**/
        // Method called by the timer at regular intervals
        private void TimerCallback(object state)
        {
            //AddProcess(); // Call the method to add a row
            LoopFunction();
        }

        private void LoopFunction()
        {
            if (dataGridView4.InvokeRequired)
            {
                // If called from a non-UI thread, invoke on the UI thread
                dataGridView4.Invoke(new Action(LoopFunction));
            }
            else
            {
                switch (schedulingPolicyNum)
                {
                    case 0:
                        ExecuteFCFS();
                        break;

                    case 1:
                        ExecuteSJF();
                        break;

                    case 2:
                        ExecutePriority();
                        break;

                    case 3:
                        ExecuteRoundRobin();
                        break;
                }

                CheckJobQueueProcesses();
                mem.VisualizeMemory();
            }
        }

        private void ExecuteFCFS()
        {
            bool processFound = false;
            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                if (dataGridView4.Rows[i].Cells[5].Value.ToString() == "Ready Queue")
                {
                    DecrementBurstTimeForRow(i); // Decrement the burst time
                    HighlightCurrentRow(i);
                    processFound = true;
                    break;
                }
            }

            if (!processFound)
            {
                // No process is ready to run
                for (int i = 0; i < dataGridView4.Rows.Count; i++)
                {
                    dataGridView4.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                Console.WriteLine("No process is ready to run.");
            }
        }

        private void ExecuteSJF()
        {
            SortTable();
            bool processFound = false;
            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                if (dataGridView4.Rows[i].Cells[5].Value.ToString() == "Ready Queue")
                {
                    DecrementBurstTimeForRow(i); // Decrement the burst time
                    HighlightCurrentRow(i);
                    processFound = true;
                    break;
                }
            }

            if (!processFound)
            {
                // No process is ready to run
                for (int i = 0; i < dataGridView4.Rows.Count; i++)
                {
                    dataGridView4.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                Console.WriteLine("No process is ready to run.");
            }
        }

        private void ExecutePriority()
        {
            SortTable();
            bool processFound = false;
            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                if (dataGridView4.Rows[i].Cells[5].Value.ToString() == "Ready Queue")
                {
                    DecrementBurstTimeForRow(i); // Decrement the burst time
                    HighlightCurrentRow(i);
                    processFound = true;
                    break;
                }
            }

            if (!processFound)
            {
                // No process is ready to run
                for (int i = 0; i < dataGridView4.Rows.Count; i++)
                {
                    dataGridView4.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                Console.WriteLine("No process is ready to run.");
            }
        }

        private void ExecuteRoundRobin()
        {
            //ResetAllStatuses();

            int checkedRows = 0;
            while (checkedRows < dataGridView4.RowCount)
            {
                currentRowIndex = (currentRowIndex + 1) % dataGridView4.RowCount;

                if (dataGridView4.Rows[currentRowIndex].Cells[5].Value.ToString() != "Job Queue")
                {
                    int remainingBurstTime = GetRemainingBurstTime(currentRowIndex);

                    if (remainingBurstTime > 0)
                    {
                        dataGridView4.Rows[currentRowIndex].Cells[5].Value = "Ready Queue";
                        HighlightCurrentRow(currentRowIndex);
                        DecrementBurstTimeForRow(currentRowIndex);
                    }
                    else
                    {
                        dataGridView4.Rows.RemoveAt(currentRowIndex);
                        currentRowIndex = (currentRowIndex - 1) % dataGridView4.RowCount;
                    }

                    break;
                }

                checkedRows++;
            }

            dataGridView4.Invalidate();
        }

        private void HighlightCurrentRow(int rowIndex)
        {
            // Clear previous selection
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {
                dataGridView4.Rows[i].DefaultCellStyle.BackColor = Color.White;
                //ResetAllStatuses();
            }

            // Highlight the current row
            if (rowIndex >= 0 && rowIndex < dataGridView4.Rows.Count)
            {
                dataGridView4.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                //dataGridView4.Rows[rowIndex].Cells[5].Value = "Running";
            }
            AllocateMemoryIfNeeded(rowIndex);
        }

        private void AllocateMemoryIfNeeded(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView4.RowCount)
            {
                int memorySize = int.Parse(dataGridView4.Rows[rowIndex].Cells[2].Value.ToString());

                if (CheckAvailableMemory(memorySize))
                {
                    // Allocate memory for the process
                    totalMemoryUsed += memorySize;
                    allocatedProcesses.Add(int.Parse(dataGridView4.Rows[rowIndex].Cells[0].Value.ToString()));
                }
            }
        }

        private bool CheckAvailableMemory(int memorySize)
        {
            // Check if there is enough available memory
            //Console.WriteLine("free Memory = " + (mem.totalMemory - (totalMemoryUsed + memorySize)));

            return (totalMemoryUsed + memorySize) <= mem.totalMemory;
        }

        private void FreeMemory(int memorySize)
        {
            // Free memory when a process finishes
            totalMemoryUsed -= memorySize;
            Console.WriteLine("Freed; TotalMemoryUsed = " + totalMemoryUsed);
        }

        private void DecrementRowBurstTime(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dataGridView4.RowCount) return;

            try
            {
                int oldBurstTimeVal = int.Parse(dataGridView4.Rows[rowIndex].Cells[1].Value.ToString());
                dataGridView4.Rows[rowIndex].Cells[1].Value = oldBurstTimeVal - 1;

                if (oldBurstTimeVal - 1 == 0)
                {
                    dataGridView4.Rows.RemoveAt(rowIndex);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
        /**private void CheckJobQueueProcesses()
        {
            // Get the total memory currently used
            int totalMemoryUsed = mem.CalculateTotalMemoryUsed();

            // Iterate through the Job Queue processes
            foreach ((int processID, int memoryRequired, int burstTime, int priority) in mem.jobQueuee)
            {
                // Check if there is enough available memory for the process
                try
                {

                    if (totalMemoryUsed + memoryRequired <= mem.totalMemory)
                    {
                        // Allocate memory for the process
                        totalMemoryUsed += memoryRequired;
                        allocatedProcesses.Add(processID);

                        // Update the status to Ready Queue
                        DataGridViewRow row = dataGridView4.Rows[processID - 1];
                        row.Cells[5].Value = "Ready Queue";
                        AllocateMemory(processID, memoryRequired, burstTime, priority);
                    }
                    else
                    {
                        // Stop checking further processes if there's no available memory
                        break;
                    }
                }catch (Exception exo)
                {
                    Console.WriteLine("wip");
                }
            }
        }**/

        private void CheckJobQueueProcesses()
        {
            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                if (dataGridView4.Rows[i].Cells[5].Value.ToString() == "Job Queue")
                {
                    Console.WriteLine("L");
                    int memorySize = int.Parse(dataGridView4.Rows[i].Cells[2].Value.ToString());
                    int processId = int.Parse(dataGridView4.Rows[i].Cells[0].Value.ToString());

                    if (memorySize <= mem.totalMemory - mem.CalculateTotalMemoryUsed())
                    {
                        // Change status to Ready
                        dataGridView4.Rows[i].Cells[5].Value = "Ready Queue";
                        //AllocateMemory(memorySize); // Allocate memory for the process
                        allocatedProcesses.Add(int.Parse(dataGridView4.Rows[i].Cells[0].Value.ToString()));
                        AllocateMemory(processId, memorySize);
                        Console.WriteLine("Process ID " + dataGridView4.Rows[i].Cells[0].Value.ToString() + " moved from Job Queue to Ready.");
                    }
                }
            }
        }

        private int GetRemainingBurstTime(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView4.RowCount)
            {
                try
                {
                    return int.Parse(dataGridView4.Rows[rowIndex].Cells[1].Value.ToString());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: Unable to parse burst time.");
                }
            }
            return 0;
        }

        private void DecrementBurstTimeForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dataGridView4.RowCount) return;

            try
            {
                int oldBurstTimeVal = int.Parse(dataGridView4.Rows[rowIndex].Cells[1].Value.ToString());
                int newBurstTimeVal = oldBurstTimeVal - timeQuantum;

                if (newBurstTimeVal <= 0)
                {
                    int memorySize = int.Parse(dataGridView4.Rows[rowIndex].Cells[2].Value.ToString());
                    int processId = int.Parse(dataGridView4.Rows[rowIndex].Cells[0].Value.ToString());
                    //FreeMemory(memorySize);
                    dataGridView4.Rows.RemoveAt(rowIndex);
                    mem.DeallocateMemory(processId);
                    if (rowIndex <= currentRowIndex)
                    {
                        currentRowIndex--;
                    }
                }
                else
                {
                    dataGridView4.Rows[rowIndex].Cells[1].Value = newBurstTimeVal;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error updating burst time: " + e.Message);
            }
        }

        private void SortTable()
        {
            List<object[]> rows = new List<object[]>();

            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                object[] rowData = new object[dataGridView4.ColumnCount];
                for (int j = 0; j < dataGridView4.ColumnCount; j++)
                {
                    rowData[j] = dataGridView4.Rows[i].Cells[j].Value;
                }
                rows.Add(rowData);
            }

            switch (schedulingPolicyNum)
            {
                case 1:
                    rows.Sort((row1, row2) => int.Parse(row1[1].ToString()).CompareTo(int.Parse(row2[1].ToString())));
                    break;
                case 2:
                    rows.Sort((row1, row2) => int.Parse(row1[4].ToString()).CompareTo(int.Parse(row2[4].ToString())));
                    break;
            }

            dataGridView4.Rows.Clear();
            foreach (object[] row in rows)
            {
                dataGridView4.Rows.Add(row);
            }
        }

        private void SetRowStatus(int rowIndex, string status)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView4.RowCount)
            {
                dataGridView4.Rows[rowIndex].Cells[5].Value = status;
                HighlightCurrentRow(rowIndex);
                dataGridView4.Invalidate();
            }
        }

        public bool AllocateMemory(int processId, int memorySize)
        {
            int numPages = (memorySize + mem.pageSize - 1) / mem.pageSize;
            if (mem.freeMemory >= memorySize)
            {
                List<int> allocatedPages = new List<int>();
                for (int i = 0; i < mem.frames.Length && allocatedPages.Count < numPages; i++)
                {
                    if (mem.frames[i] == -1)
                    {
                        mem.frames[i] = processId;
                        allocatedPages.Add(i);
                    }
                }

                if (allocatedPages.Count == numPages)
                {
                    mem.freeMemory -= memorySize;
                    mem.readyQueue.Add(processId);  // Add to ready queue
                    //UpdateQueues(); // Update the display of queues
                    Console.WriteLine($"Allocated {memorySize} memory to Process {processId}");
                    return true;
                }
            }

            mem.jobQueuee.Add((processId, memorySize));  // Add to job queue
            //UpdateQueues(); // Update the display of queues
            Console.WriteLine($"Process {processId} added to job queue due to insufficient memory.");
            return false;
        }

        private void ResetAllStatuses()
        {
            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                int memorySize = int.Parse(dataGridView4.Rows[i].Cells[2].Value.ToString());

                if (mem.CheckAvailableMemory(memorySize))
                {
                    dataGridView4.Rows[i].Cells[5].Value = "Ready Queue";
                }
                else
                {
                    dataGridView4.Rows[i].Cells[5].Value = "Job Queue";
                }
            }
        }

        private void ClearTableData()
        {
            dataGridView4.Rows.Clear();
            processID = 0;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AddProcess();
        }

        public void FCFS()
        {
            ClearTableData();
            schedulingPolicyNum = 0;
            initialTime = DateTime.Now;
        }

        public void SJF()
        {
            ClearTableData();
            schedulingPolicyNum = 1;
            initialTime = DateTime.Now;
        }

        public void PRIORITY()
        {
            ClearTableData();
            schedulingPolicyNum = 2;
            initialTime = DateTime.Now;
        }

        public void RR()
        {
            ClearTableData();
            schedulingPolicyNum = 3;
            initialTime = DateTime.Now;
        }

        private string ProcessIdGenerator()
        {
            return processID++.ToString();
        }

        private string BurstTimeGenerator()
        {
            Random random = new Random();
            return (random.Next(14) + 1).ToString();
        }

        private string MemorySizeGenerator()
        {
            Random random = new Random();
            return (random.Next(64) + 1).ToString();
        }

        private string ArrivalTimeGenerator()
        {
            TimeSpan timePassed = DateTime.Now - initialTime;
            return ((int)timePassed.TotalSeconds).ToString();
        }

        private string PriorityGenerator()
        {
            Random random = new Random();
            return (random.Next(9) + 1).ToString();
        }

        private string StatusGenerator(DataGridViewRow row)
        {
            int memorySize = int.Parse(row.Cells[2].Value.ToString());

            if (CheckAvailableMemory(memorySize))
            {
                return "Ready";
            }
            else
            {
                return "Job Queue";
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}