using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class paging : Form
    {
        private List<ImageTextObject> ImageTextList;
        private List<PageFrameInfo> PageFrameList;
        private List<QueueInfo> QueueInfoList;
        private Image image1;


        private System.Threading.Timer timer; // Timer for periodic updates
        private int processID = 0;
        private DateTime initialTime = DateTime.Now;
        private int schedulingPolicyNum = 0;
        private int currentRowIndex = 0;
        private int previousRowIndex = 0;
        private int timeQuantum = 2;


        public paging()
        {
            InitializeComponent();
            InitializeTimer();
            InitializeDataGridView();
            InitializeDataGridView2();
            InitializeDataGridView3();
            

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
                cell6.Value = StatusGenerator();
                row.Cells.Add(cell6);
                dataGridView4.Rows.Add(row);
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

                Console.WriteLine("schedulingPolicyNum is " + schedulingPolicyNum);
                Console.WriteLine("current time : " + DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                switch (schedulingPolicyNum)
                {
                    case 0:
                        if (dataGridView4.RowCount > 0)
                        {
                            DecrementTopRowBurstTime();
                        }
                        AddProcess();
                        SetTopRowStatus("Running");
                        break;

                    case 1:
                        if (dataGridView4.RowCount > 0)
                        {
                            DecrementTopRowBurstTime();
                        }

                        AddProcess();
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
                        AddProcess();
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
                // Add five processes
                for (int i = 0; i < 5; i++)
                {
                    AddProcess();
                }
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
            if (rowIndex >= 0 && rowIndex < dataGridView4.RowCount)
            {
                try
                {
                    int oldBurstTimeVal = int.Parse(dataGridView4.Rows[rowIndex].Cells[1].Value.ToString());
                    int newBurstTimeVal = oldBurstTimeVal - timeQuantum;

                    if (newBurstTimeVal <= 0)
                    {
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
            return (random.Next(100) + 1).ToString();
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

        public void UpdateDataGrid(int[] frames)
        {
            ImageTextList = new List<ImageTextObject>();
            PageFrameList = new List<PageFrameInfo>();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] == -1)
                {
                    ImageTextList.Add(new ImageTextObject(image1, "FREE"));
                    PageFrameList.Add(new PageFrameInfo { PageNumber = i, FrameNumber = "N/A", InMemory = false });
                }
                else
                {
                    ImageTextList.Add(new ImageTextObject(image1, frames[i].ToString()));
                    PageFrameList.Add(new PageFrameInfo { PageNumber = i, FrameNumber = frames[i].ToString(), InMemory = true });
                }
            }

            dataGridView1.DataSource = null; // Reset the data source
            dataGridView1.DataSource = ImageTextList; // Set the new data source

            dataGridView2.DataSource = null; // Reset the data source
            dataGridView2.DataSource = PageFrameList; // Set the new data source

            // Manually trigger cell formatting for each cell
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    FormatCell(i, j);
                }
            }

            dataGridView1.Refresh();
            dataGridView2.Refresh();
        }

        public void UpdateQueueGrid(Queue<(int processId, int memorySize)> jobQueue, List<int> readyQueue)
        {
            QueueInfoList = new List<QueueInfo>();

            foreach (var job in jobQueue)
            {
                QueueInfoList.Add(new QueueInfo { QueueType = "Job Queue", ProcessId = job.processId, MemorySize = job.memorySize });
            }

            foreach (var processId in readyQueue)
            {
                QueueInfoList.Add(new QueueInfo { QueueType = "Ready Queue", ProcessId = processId, MemorySize = 0 }); // Assuming memory size is not needed for ready queue
            }

            dataGridView3.DataSource = null; // Reset the data source
            dataGridView3.DataSource = QueueInfoList; // Set the new data source

            dataGridView3.Refresh();
        }

        private void FormatCell(int rowIndex, int columnIndex)
        {
            if (columnIndex == 0) // Assuming the first column holds the ImageTextObject
            {
                ImageTextObject imageTextObject = (ImageTextObject)dataGridView1.Rows[rowIndex].DataBoundItem;

                // Check if the cell value is "FREE"
                if (imageTextObject.Text == "FREE")
                {
                    // Set the cell's background color to green
                    dataGridView1.Rows[rowIndex].Cells[columnIndex].Style.BackColor = Color.LightGreen;
                }
                else
                {
                    // Set the cell's background color to red
                    dataGridView1.Rows[rowIndex].Cells[columnIndex].Style.BackColor = Color.Red;
                }
            }
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            FormatCell(e.RowIndex, e.ColumnIndex);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowHeadersVisible = false; // Hide row headers
            dataGridView1.ColumnHeadersVisible = false; // Hide column headers

            // Configure the DataGridView columns
            DataGridViewTextBoxColumn imageTextColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Text" // This can be any property; the drawing will be handled in CellPainting
            };
            dataGridView1.Columns.Add(imageTextColumn);

            // Subscribe to the SelectionChanged event
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;

        }

        private void InitializeDataGridView2()
        {
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.RowHeadersVisible = false; // Hide row headers

            // Set the header style
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView2.EnableHeadersVisualStyles = false;
            // Set a consistent row height
            dataGridView2.RowTemplate.Height = 30;

            // Configure the DataGridView columns
            DataGridViewTextBoxColumn pageNumberColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Page No.",
                DataPropertyName = "PageNumber"
            };
            dataGridView2.Columns.Add(pageNumberColumn);

            DataGridViewTextBoxColumn frameNumberColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Frame No.",
                DataPropertyName = "FrameNumber"
            };
            dataGridView2.Columns.Add(frameNumberColumn);

            DataGridViewTextBoxColumn inMemoryColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "In Memory",
                DataPropertyName = "InMemory"
            };
            dataGridView2.Columns.Add(inMemoryColumn);

            // Subscribe to the SelectionChanged event
            dataGridView2.SelectionChanged += DataGridView2_SelectionChanged;
        }


        private void InitializeDataGridView3()
        {
            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.RowHeadersVisible = false; // Hide row headers

            // Set the header style
            dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView3.EnableHeadersVisualStyles = false;
            // Set a consistent row height
            dataGridView3.RowTemplate.Height = 30;

            // Configure the DataGridView columns
            DataGridViewTextBoxColumn queueTypeColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Queue Type",
                DataPropertyName = "QueueType"
            };
            dataGridView3.Columns.Add(queueTypeColumn);

            DataGridViewTextBoxColumn processIdColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Process ID",
                DataPropertyName = "ProcessId"
            };
            dataGridView3.Columns.Add(processIdColumn);

            DataGridViewTextBoxColumn memorySizeColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Memory Size",
                DataPropertyName = "MemorySize"
            };
            dataGridView3.Columns.Add(memorySizeColumn);

            // Subscribe to the SelectionChanged event
            dataGridView3.SelectionChanged += DataGridView3_SelectionChanged;
        }


        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Clear the selection to remove the blue color indicating an active cell
            dataGridView1.ClearSelection();
        }

        private void DataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            // Clear the selection to remove the blue color indicating an active cell
            dataGridView2.ClearSelection();
        }

        private void DataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            // Clear the selection to remove the blue color indicating an active cell
            dataGridView3.ClearSelection();
        }

        private void paging_Load(object sender, EventArgs e)
        {
            // Any additional initialization code
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

  
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearTableData();
            schedulingPolicyNum = 0;
            initialTime = DateTime.Now;
            label1.Text = "FCFS";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearTableData();
            schedulingPolicyNum = 1;
            initialTime = DateTime.Now;
            label1.Text = "SJF";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearTableData();
            schedulingPolicyNum = 2;
            initialTime = DateTime.Now;
            label1.Text = "PRIORITY";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearTableData();
            schedulingPolicyNum = 3;
            initialTime = DateTime.Now;
            label1.Text = "RR";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddProcess();
        }
    }

    public class ImageTextObject
    {
        public Image Image { get; set; }
        public string Text { get; set; }

        public ImageTextObject(Image image, string text)
        {
            Image = image;
            Text = text;
        }
    }

    public class PageFrameInfo
    {
        public int PageNumber { get; set; }
        public string FrameNumber { get; set; }
        public bool InMemory { get; set; }
    }

    public class QueueInfo
    {
        public string QueueType { get; set; }
        public int ProcessId { get; set; }
        public int MemorySize { get; set; }
    }
}
