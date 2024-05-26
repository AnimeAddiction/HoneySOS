using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        public DataGridView dataGridView4;
        public paging page;
        private List<int> allocatedProcesses = new List<int>();

        private int memorySize;

        public Form1(DataGridView datagridview, MemoryManager memoryManager, int memorySize)
        {

            InitializeTimer(); // Initialize the timer
            dataGridView4 = datagridview;

            mem = memoryManager;
            this.memorySize = memorySize;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
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
                String processId = ProcessIdGenerator();
                cell1.Value = processId;
                row.Cells.Add(cell1);

                DataGridViewCell cell2 = new DataGridViewTextBoxCell();
                cell2.Value = BurstTimeGenerator();
                row.Cells.Add(cell2);

                DataGridViewCell cell3 = new DataGridViewTextBoxCell();
                String memorySize = MemorySizeGenerator();
                cell3.Value = memorySize;
                row.Cells.Add(cell3);

                DataGridViewCell cell4 = new DataGridViewTextBoxCell();
                cell4.Value = ArrivalTimeGenerator();
                row.Cells.Add(cell4);

                DataGridViewCell cell5 = new DataGridViewTextBoxCell();
                cell5.Value = PriorityGenerator();
                row.Cells.Add(cell5);


                DataGridViewCell cell6 = new DataGridViewTextBoxCell();
                cell6.Value = StatusGenerator();
                row.Cells.Add(cell6);
                dataGridView4.Rows.Add(row);

                Console.WriteLine(int.Parse(memorySize) + "asdasdsadsadsad");
                mem.AllocateMemory(int.Parse(processId), int.Parse(memorySize));
                mem.VisualizeMemory();
            }
        }

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
                //MessageBox.Show("This is a message displayed to the user.");

                //Console.WriteLine("schedulingPolicyNum is " + schedulingPolicyNum);
                //Console.WriteLine("current time : " + DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                switch (schedulingPolicyNum)
                {
                    case 0:
                        if (dataGridView4.RowCount > 0)
                        {
                            DecrementTopRowBurstTime();
                        }
                        //AddProcess();
                        SetTopRowStatus("Running");
                        break;

                    case 1:
                        if (dataGridView4.RowCount > 0)
                        {
                            DecrementTopRowBurstTime();
                        }

                        //AddProcess();
                        SortTable();
                        SetTopRowStatus("Running");
                        break;

                    case 2:
                        currentRowIndex = 0;
                        if (dataGridView4.RowCount > 0)
                        {
                            DecrementTopRowBurstTime();
                            HighlightCurrentRow(currentRowIndex);
                        }
                        //AddProcess();
                        SortTable();
                        SetTopRowStatus("Running");
                        break;

                    case 3:
                        if (dataGridView4.RowCount > 0)
                        {
                            ExecuteRoundRobin();
                        }
                        break;
                        //                addProcess();
                        // This is by process number
                }
            }
        }

        private void HighlightCurrentRow(int rowIndex)
        {
            // Clear previous selection
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {
                dataGridView4.Rows[i].DefaultCellStyle.BackColor = Color.White;
            }

            // Highlight the current row
            if (rowIndex >= 0 && rowIndex < dataGridView4.Rows.Count)
            {
                dataGridView4.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            //AllocateMemoryIfNeeded(rowIndex);
            
        }
        private void AllocateMemoryIfNeeded(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView4.RowCount)
            {
                int processId = int.Parse(dataGridView4.Rows[rowIndex].Cells[0].Value.ToString());

                if (!allocatedProcesses.Contains(processId))
                {
                    int memorySize = int.Parse(dataGridView4.Rows[rowIndex].Cells[2].Value.ToString());
                    mem.AllocateMemory(processId, memorySize);
                    allocatedProcesses.Add(processId); // Add process to the list after allocating memory
                }
            }
        }
        private void DecrementTopRowBurstTime()
        {
            if (dataGridView4.RowCount > 0)
            {
                try
                {
                    int columnIndex = 1; // Adjust column index as needed
                    object cellValue = dataGridView4.Rows[0].Cells[columnIndex].Value;

                    if (cellValue != null && int.TryParse(cellValue.ToString(), out int oldBurstTimeVal))
                    {
                        dataGridView4.Rows[0].Cells[columnIndex].Value = oldBurstTimeVal - 1;

                        if (oldBurstTimeVal - 1 == 0)
                        {
                            mem.DeallocateMemory(int.Parse(dataGridView4.Rows[0].Cells[0].Value.ToString()));
                            Console.WriteLine("Memorasdadsad" + int.Parse(dataGridView4.Rows[0].Cells[0].Value.ToString()));
                            mem.VisualizeMemory();
                            // Handle case when the decremented value is 0
                            dataGridView4.Rows.RemoveAt(0);
                        }
                    }
                    else
                    {
                        // Handle case when cell value is null or cannot be parsed to int
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    // Handle the exception (e.g., log it, display an error message, etc.)
                    Console.WriteLine("12");
                    // e.printStackTrace(); // Print stack trace for debugging
                }
            }
        }

        private void ExecuteRoundRobin()
        {
            if (dataGridView4.RowCount == 0)
            {
                //return; // If there are no processes in the queue, return immediately
            }

            if (dataGridView4.RowCount == 1)
            {
                AddProcess();              
            }
            // Reset all statuses to "Ready"
            ResetAllStatuses();

            int remainingBurstTime = GetRemainingBurstTime(currentRowIndex);

            if (remainingBurstTime > 0)
            {
                // If the current process still has remaining burst time, continue running it
                dataGridView4.Rows[currentRowIndex].Cells[5].Value = "Running";
                HighlightCurrentRow(currentRowIndex);
                DecrementBurstTimeForRow(currentRowIndex); // Decrement burst time for the current process
            }
            else
            {
                // If the current process has completed its burst time, remove it from the queue
                dataGridView4.Rows.RemoveAt(currentRowIndex);
            }

            // Move to the next process in the queue
            currentRowIndex = (currentRowIndex + 1) % dataGridView4.RowCount;

            // Update status to "Running" for the next process
            //dataGridView1.Rows[currentRowIndex].Cells[5].Value = "Running";

            // If you're using a custom renderer, update it here
            //highlightRenderer.SetTopRowIndex(currentRowIndex); // Highlight the current row
            // If jTable1 is a DataGridView, you might not need to call jTable1.repaint()
            dataGridView4.Invalidate(); // Repaint the table to apply the changes
        }


        private int GetRemainingBurstTime(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView4.RowCount)
            {
                try
                {
                    int remainingBurstTime = int.Parse(dataGridView4.Rows[rowIndex].Cells[1].Value.ToString());
                    return remainingBurstTime;
                }
                catch (FormatException)
                {
                    // Handle format exception
                    Console.WriteLine("Error: Unable to parse burst time.");
                }
            }
            return 0; // Default return value if rowIndex is out of range or parsing fails
        }

        private void DecrementBurstTimeForRow(int rowIndex)
        {
            Console.WriteLine("HI");
            if (rowIndex >= 0 && rowIndex < dataGridView4.RowCount)
            {
                try
                {
                    int oldBurstTimeVal = int.Parse(dataGridView4.Rows[rowIndex].Cells[1].Value.ToString());
                    int newBurstTimeVal = oldBurstTimeVal - timeQuantum;

                    if (newBurstTimeVal <= 0)
                    {

                        mem.DeallocateMemory(int.Parse(dataGridView4.Rows[rowIndex].Cells[0].Value.ToString()));
                        Console.WriteLine("Memorasdadsad" + int.Parse(dataGridView4.Rows[rowIndex].Cells[0].Value.ToString()));
                        mem.VisualizeMemory();
                        // If the decremented burst time is zero or negative, remove the row
                        dataGridView4.Rows.RemoveAt(rowIndex);

                        
                        // Adjust the currentRowIndex if the deleted row is before it
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
                    // Handle the exception
                    Console.WriteLine("Error updating burst time: " + e.Message);
                }
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
                case 1: // Assuming 1 is for sorting by priority
                    rows.Sort((row1, row2) => int.Parse(row1[1].ToString()).CompareTo(int.Parse(row2[1].ToString())));
                    break;
                case 2: // Add other sorting policies if needed
                    rows.Sort((row1, row2) => int.Parse(row1[4].ToString()).CompareTo(int.Parse(row2[4].ToString())));
                    break;
            }

            dataGridView4.Rows.Clear();
            foreach (object[] row in rows)
            {
                dataGridView4.Rows.Add(row);
            }
        }


        private void SetTopRowStatus(string status)
        {
            if (dataGridView4.RowCount > 0)
            {
                ResetAllStatuses();
                dataGridView4.Rows[0].Cells[5].Value = status;
                HighlightCurrentRow(currentRowIndex);
                // If you're using a custom renderer, update it here
                // If you're using a DataGridView instead of jTable1, use dataGridView1 instead of jTable1
                // If jTable1 is a DataGridView, you might not need to call jTable1.repaint()
                dataGridView4.Invalidate(); // Repaint the table to apply the changes
            }
        }

        private void ResetAllStatuses()
        {
            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                dataGridView4.Rows[i].Cells[5].Value = "Ready";
            }
            // If you're using a custom renderer, update it here
            //highlightRenderer.SetTopRowIndex(-1); // Reset top row index in renderer
        }

        // Button click event handler to manually add a row

        private void button1_Click(object sender, EventArgs e)
        {
            //AddRowToTable();
        }

        // Form load event handler (optional)
        private void Form1_Load(object sender, EventArgs e)
        {
            // Additional initialization code can be added here
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
            return (random.Next(1, memorySize)).ToString();
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

        private string StatusGenerator()
        {
            return "Ready";
        }

        private void ClearTableData()
        {
            // Clear scheduling table
            dataGridView4.Rows.Clear();

            // Reset processID
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
    }
}


