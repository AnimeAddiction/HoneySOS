using System;
using System.Collections.Generic;
using System.Drawing;
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
        private const int timeQuantum = 2;
        private readonly MemoryManager mem;
        private volatile bool isPaused = false; // Flag to control loop executio

        public DataGridView dataGridView4;
        public paging page;
        private readonly List<int> allocatedProcesses = new List<int>();

        private readonly int memorySize;

        private readonly List<ProcessInfo> processList = new List<ProcessInfo>();

        public Form1(DataGridView datagridview, MemoryManager memoryManager, int memorySize)
        {
            InitializeComponent();
            InitializeTimer(); // Initialize the timer
            dataGridView4 = datagridview;

            mem = memoryManager;
            this.memorySize = memorySize;
        }

        private void InitializeTimer()
        {
            // Create a timer that calls TimerCallback every 1000 milliseconds
            timer = new System.Threading.Timer(TimerCallback, null, 0, 1000);
        }

        private void UpdateDataGridView()
        {
            if (dataGridView4.InvokeRequired)
            {
                // If called from a non-UI thread, invoke on the UI thread
                dataGridView4.Invoke(new Action(UpdateDataGridView));
            }
            else
            {
                dataGridView4.Rows.Clear();
                foreach (var process in processList)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.AddRange(
                        new DataGridViewTextBoxCell { Value = process.ProcessId },
                        new DataGridViewTextBoxCell { Value = process.BurstTime },
                        new DataGridViewTextBoxCell { Value = process.MemorySize },
                        new DataGridViewTextBoxCell { Value = process.ArrivalTime },
                        new DataGridViewTextBoxCell { Value = process.Priority },
                        new DataGridViewTextBoxCell { Value = process.Status }
                    );

                    UpdateProcessStatus();
                    dataGridView4.Rows.Add(row);
                }
            }
        }

        public void UpdateProcessStatus()
        {
            // Iterate through the processes in the ready queue and update their status to "Ready"
            foreach (var processId in mem.readyMemoryMap.Keys)
            {
                var process = processList.Find(p => p.ProcessId == processId.ToString());
                if (process != null)
                {
                    process.Status = "Ready";
                }
            }

            // Iterate through the processes in the job queue and update their status to "Waiting"
            foreach (var job in mem.jobQueue)
            {
                var process = processList.Find(p => p.ProcessId == job.processId.ToString());
                if (process != null)
                {
                    process.Status = "Waiting";
                }
            }
        }

            // Method to add a process to the list and DataGridView
        public void AddProcess()
        {
            PauseLoop();
            var process = new ProcessInfo
            {
                ProcessId = ProcessIdGenerator(),
                BurstTime = BurstTimeGenerator(),
                MemorySize = MemorySizeGenerator(),
                ArrivalTime = ArrivalTimeGenerator(),
                Priority = PriorityGenerator(),
                Status = StatusGenerator()
            };

            processList.Add(process);
            UpdateDataGridView();

            switch (schedulingPolicyNum)
            {
                case 0:
                    Console.WriteLine("schedule number:  " + schedulingPolicyNum);
                    mem.jobQueue.Enqueue((int.Parse(process.ProcessId), process.MemorySize));
                    mem.FCFS();
                    mem.VisualizeMemory();
                    break;

                case 1:
                    Console.WriteLine("schedule number:  " + schedulingPolicyNum);
                    mem.SJF(processList);
                    mem.VisualizeMemory();
                    break;

                case 2:
                    Console.WriteLine("schedule number:  " + schedulingPolicyNum);
                    mem.Priority(processList);
                    mem.VisualizeMemory();
                    break;

                case 3:
                    // Handle other scheduling policies here
                    break;
            }
            ResumeLoop();
        }

        private void TimerCallback(object state)
        {
            LoopFunction();
        }

        private void LoopFunction()
        {
            if (!isPaused) // Check if the loop is not paused
            {
                switch (schedulingPolicyNum)
                {
                    case 0:
                        if (processList.Count > 0)
                        {
                            DecrementTopRowBurstTime();
                        }
                        SetTopRowStatus("Running");
                        break;

                    case 1:
                        if (processList.Count > 0)
                        {
                            DecrementTopRowBurstTime();
                            SortTable();
                        }
                        SetTopRowStatus("Running");
                        break;

                    case 2:
                        currentRowIndex = 0;
                        if (processList.Count > 0)
                        {
                            DecrementTopRowBurstTime();
                            HighlightCurrentRow(currentRowIndex);
                        }
                        SortTable();
                        SetTopRowStatus("Running");
                        break;

                    case 3:
                        if (processList.Count > 0)
                        {
                            ExecuteRoundRobin();
                        }
                        break;
                }
            }
        }

        public void PauseLoop()
        {
            isPaused = true; // Set the flag to pause the loop
        }

        public void ResumeLoop()
        {
            isPaused = false; // Set the flag to resume the loop
        }

        private void HighlightCurrentRow(int rowIndex)
        {
            // Clear previous selection in DataGridView
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {
                dataGridView4.Rows[i].DefaultCellStyle.BackColor = Color.White;
            }

            // Highlight the current row
            if (rowIndex >= 0 && rowIndex < dataGridView4.Rows.Count)
            {
                dataGridView4.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
        }

        private void DecrementTopRowBurstTime()
        {
            if (processList.Count > 0)
            {
                var process = processList[0];
                process.BurstTime -= 1;

                if (process.BurstTime <= 0)
                {
                    // Remove the process from the process list when its burst time reaches zero
                    processList.RemoveAt(0);
                    mem.DeallocateMemory(int.Parse(process.ProcessId));
                    mem.VisualizeMemory();
                }

                // Update the DataGridView
                UpdateDataGridView();
            }
        }

        private void ExecuteRoundRobin()
        {
            if (processList.Count == 0)
            {
                return; // If there are no processes in the queue, return immediately
            }

            // Reset all statuses to "Ready"
            foreach (var process in processList)
            {
                process.Status = "Ready";
            }

            int remainingBurstTime = GetRemainingBurstTime(currentRowIndex);

            if (remainingBurstTime > 0)
            {
                // If the current process still has remaining burst time, continue running it
                processList[currentRowIndex].Status = "Running";
                HighlightCurrentRow(currentRowIndex);
                DecrementBurstTimeForRow(currentRowIndex); // Decrement burst time for the current process
            }
            else
            {
                // If the current process has completed its burst time, remove it from the queue
                processList.RemoveAt(currentRowIndex);
            }

            // Move to the next process in the queue
            if (processList.Count > 0)
            {
                currentRowIndex = (currentRowIndex + 1) % processList.Count;
            }

            // Update the DataGridView
            UpdateDataGridView();
        }

        private int GetRemainingBurstTime(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < processList.Count)
            {
                return processList[rowIndex].BurstTime;
            }
            return 0; // Default return value if rowIndex is out of range
        }

        private void DecrementBurstTimeForRow(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < processList.Count)
            {
                var process = processList[rowIndex];
                process.BurstTime -= timeQuantum;

                if (process.BurstTime <= 0)
                {
                    processList.RemoveAt(rowIndex);
                    mem.DeallocateMemory(int.Parse(process.ProcessId));
                    mem.VisualizeMemory();

                    // Adjust the currentRowIndex if the deleted row is before it
                    if (rowIndex <= currentRowIndex && currentRowIndex > 0)
                    {
                        currentRowIndex--;
                    }
                }

                // Update the DataGridView
                UpdateDataGridView();
            }
        }

        private void SortTable()
        {
            switch (schedulingPolicyNum)
            {
                case 1: // Shortest Job First
                    processList.Sort((p1, p2) => p1.BurstTime.CompareTo(p2.BurstTime));
                    break;
                case 2: // Priority Scheduling
                    processList.Sort((p1, p2) => p1.Priority.CompareTo(p2.Priority));
                    break;
            }

            // Update the DataGridView
            UpdateDataGridView();
        }

        private void SetTopRowStatus(string status)
        {
            if (processList.Count > 0)
            {
                foreach (var process in processList)
                {
                    process.Status = "Ready";
                }

                var topProcess = processList[0];
                topProcess.Status = status;

                HighlightCurrentRow(currentRowIndex);
                UpdateDataGridView();
            }
        }

        private void InitializeComponent()
        {
            // Your form initialization code here
        }

        private string ProcessIdGenerator()
        {
            return processID++.ToString();
        }

        private int BurstTimeGenerator()
        {
            Random random = new Random();
            return random.Next(14) + 1;
        }

        private int MemorySizeGenerator()
        {
            Random random = new Random();
            return random.Next(1, memorySize);
        }

        private int ArrivalTimeGenerator()
        {
            TimeSpan timePassed = DateTime.Now - initialTime;
            return (int)timePassed.TotalSeconds;
        }

        private int PriorityGenerator()
        {
            Random random = new Random();
            return random.Next(9) + 1;
        }

        private string StatusGenerator()
        {
            return "Ready";
        }

        private void ClearTableData()
        {
            processList.Clear();
            processID = 0;
            UpdateDataGridView();
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


