/*using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class HoneyOS : Form
    {
        private List<ImageTextObject> ImageTextList;
        private List<PageFrameInfo> PageFrameList;
        private List<QueueInfo> QueueInfoList;
        private Image image1;
        MemoryManager memoryManager;
        public Form1 sched;
        
        public HoneyOS()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeDataGridView2();
            //InitializeDataGridView3();   
            memoryManager = new MemoryManager(256, 4, this);
            sched = new Form1(dataGridView4, memoryManager);
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

            //dataGridView3.DataSource = null; // Reset the data source
            //dataGridView3.DataSource = QueueInfoList; // Set the new data source

            //dataGridView3.Refresh();
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


        /**private void InitializeDataGridView3()
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
        }**/


    /*   private void DataGridView1_SelectionChanged(object sender, EventArgs e)
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
            //dataGridView3.ClearSelection();
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
            sched.AddProcess();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            memoryManager.ClearFrames();
            sched.FCFS();
            label1.Text = "First Come First Serve";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            memoryManager.ClearFrames();
            sched.SJF();
            label1.Text = "Shortest Job First";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            memoryManager.ClearFrames();
            sched.PRIORITY();
            label1.Text = "Priority";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            memoryManager.ClearFrames();
            sched.RR();
            label1.Text = "Round Robin";
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

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
 */