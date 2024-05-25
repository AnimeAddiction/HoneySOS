using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class paging : Form
    {
        private List<ImageTextObject> ImageTextList;
        private Image image1;

        public paging()
        {
            InitializeComponent();
            InitializeDataGridView();
            
        }

        public void UpdateDataGrid(int[] frames)
        {
            ImageTextList = new List<ImageTextObject>();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] == -1)
                {
                    ImageTextList.Add(new ImageTextObject(image1, "FREE"));
                }
                else
                {
                    ImageTextList.Add(new ImageTextObject(image1, frames[i].ToString()));
                }
            }

            dataGridView1.DataSource = null; // Reset the data source
            dataGridView1.DataSource = ImageTextList; // Set the new data source

            // Manually trigger cell formatting for each cell
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    FormatCell(i, j);
                }
            }

            dataGridView1.Refresh();
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
            dataGridView1.ColumnHeadersVisible = false;  // Hide column headers

            // Configure the DataGridView columns
            DataGridViewTextBoxColumn imageTextColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Text" // This can be any property; the drawing will be handled in CellPainting
            };
            dataGridView1.Columns.Add(imageTextColumn);

            // Subscribe to the SelectionChanged event
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Clear the selection to remove the blue color indicating an active cell
            dataGridView1.ClearSelection();
        }


        private void paging_Load(object sender, EventArgs e)
        {
            // Any additional initialization code
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
}
